using Fastkart.Models.Entities;
using Fastkart.Models.EF;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fastkart.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public CartService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        // Helper: Lấy UserUid hiện tại (trả về null nếu chưa login)
        private int? GetCurrentUserUid()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && user.Identity.IsAuthenticated)
            {
                var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (idClaim != null && int.TryParse(idClaim.Value, out int userId))
                {
                    return userId;
                }
            }
            return null;
        }

        // Helper: Lấy hoặc tạo Cart cho User (Header)
        private async Task<Cart> GetOrCreateCartForUser(int userUid)
        {
            var cart = await _context.Cart
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserUid == userUid);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserUid = userUid
                };
                _context.Cart.Add(cart);
                await _context.SaveChangesAsync();
            }
            return cart;
        }

        // --- 1. LẤY GIỎ HÀNG ---
        public async Task<List<CartItemViewModel>> GetCartItemsAsync()
        {
            var userUid = GetCurrentUserUid();

            // A. Đã đăng nhập -> Lấy từ DB
            if (userUid.HasValue)
            {
                var cart = await _context.Cart
                    .Include(c => c.Items)
                        .ThenInclude(i => i.Product) // Join bảng Product lấy info
                    .FirstOrDefaultAsync(c => c.UserUid == userUid.Value);

                if (cart == null || cart.Items == null)
                    return new List<CartItemViewModel>();

                // Map từ Entity CartItem sang ViewModel
                return cart.Items.Select(i => new CartItemViewModel
                {
                    ProductId = i.ProductUid,
                    ProductName = i.Product.ProductName,
                    Image = i.Product.Thumbnail, // Xử lý parse JSON ảnh nếu cần

                    // --- TÍNH GIÁ SALE (Price - Discount) ---
                    Price = (long)(i.Product.Price - (i.Product.Price * (i.Product.Discount ?? 0) / 100)),
                    // ----------------------------------------

                    Quantity = i.Quantity
                }).ToList();
            }

            // B. Chưa đăng nhập -> Lấy từ Session
            var json = Session.GetString("Cart");
            if (string.IsNullOrEmpty(json))
            {
                return new List<CartItemViewModel>();
            }
            return JsonConvert.DeserializeObject<List<CartItemViewModel>>(json);
        }

        // --- 2. THÊM VÀO GIỎ ---
        public async Task AddToCartAsync(int productId, int quantity = 1)
        {
            var userUid = GetCurrentUserUid();

            // A. Đã đăng nhập -> Lưu vào DB
            if (userUid.HasValue)
            {
                var cart = await GetOrCreateCartForUser(userUid.Value);

                var existingItem = await _context.CartItem
                    .FirstOrDefaultAsync(ci => ci.CartUid == cart.Uid && ci.ProductUid == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    _context.CartItem.Update(existingItem);
                }
                else
                {
                    var newItem = new CartItem
                    {
                        CartUid = cart.Uid,
                        ProductUid = productId,
                        Quantity = quantity
                    };
                    _context.CartItem.Add(newItem);
                }
                await _context.SaveChangesAsync();
            }
            // B. Chưa đăng nhập -> Lưu vào Session
            else
            {
                var cartItems = await GetCartItemsAsync();
                var existingItem = cartItems.FirstOrDefault(item => item.ProductId == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    var product = await _context.Product.FirstOrDefaultAsync(p => p.Uid == productId);
                    if (product != null)
                    {
                        // --- TÍNH GIÁ SALE KHI THÊM VÀO SESSION ---
                        long discountedPrice = (long)(product.Price - (product.Price * (product.Discount ?? 0) / 100));
                        // ------------------------------------------

                        cartItems.Add(new CartItemViewModel
                        {
                            ProductId = product.Uid,
                            ProductName = product.ProductName,
                            Image = product.Thumbnail,
                            Price = discountedPrice, // Lưu giá đã giảm
                            Quantity = quantity
                        });
                    }
                }
                SaveCartItemsSession(cartItems);
            }
        }

        // --- 3. XÓA KHỎI GIỎ ---
        public async Task RemoveFromCartAsync(int productId)
        {
            var userUid = GetCurrentUserUid();

            if (userUid.HasValue)
            {
                var cart = await _context.Cart.FirstOrDefaultAsync(c => c.UserUid == userUid.Value);
                if (cart != null)
                {
                    var item = await _context.CartItem
                        .FirstOrDefaultAsync(ci => ci.CartUid == cart.Uid && ci.ProductUid == productId);

                    if (item != null)
                    {
                        _context.CartItem.Remove(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else
            {
                var cartItems = await GetCartItemsAsync();
                var itemToRemove = cartItems.FirstOrDefault(item => item.ProductId == productId);

                if (itemToRemove != null)
                {
                    cartItems.Remove(itemToRemove);
                    SaveCartItemsSession(cartItems);
                }
            }
        }

        // --- 4. MERGE SESSION -> DATABASE (GỌI KHI LOGIN) ---
        public async Task MergeSessionCartToDatabase(int userUid)
        {
            var json = Session.GetString("Cart");
            if (string.IsNullOrEmpty(json)) return;

            var sessionItems = JsonConvert.DeserializeObject<List<CartItemViewModel>>(json);
            if (sessionItems == null || !sessionItems.Any()) return;

            var dbCart = await GetOrCreateCartForUser(userUid);

            foreach (var sItem in sessionItems)
            {
                var dbItem = dbCart.Items.FirstOrDefault(i => i.ProductUid == sItem.ProductId);

                if (dbItem != null)
                {
                    dbItem.Quantity += sItem.Quantity;
                    _context.CartItem.Update(dbItem);
                }
                else
                {
                    _context.CartItem.Add(new CartItem
                    {
                        CartUid = dbCart.Uid,
                        ProductUid = sItem.ProductId,
                        Quantity = sItem.Quantity
                    });
                }
            }

            await _context.SaveChangesAsync();
            Session.Remove("Cart");
        }

        // --- 5. TÍNH TỔNG TIỀN ---
        public async Task<long> GetSubtotalAsync()
        {
            var items = await GetCartItemsAsync();
            return items.Sum(item => item.Total);
        }

        // --- 6. XÓA SẠCH GIỎ HÀNG (Dùng sau khi Checkout) ---
        public async Task ClearCartAsync()
        {
            var userUid = GetCurrentUserUid();

            if (userUid.HasValue)
            {
                var cart = await _context.Cart.FirstOrDefaultAsync(c => c.UserUid == userUid.Value);
                if (cart != null)
                {
                    var items = _context.CartItem.Where(ci => ci.CartUid == cart.Uid);
                    _context.CartItem.RemoveRange(items);
                    await _context.SaveChangesAsync();
                }
            }

            Session.Remove("Cart");
        }

        // Helper private lưu Session
        private void SaveCartItemsSession(List<CartItemViewModel> items)
        {
            var json = JsonConvert.SerializeObject(items);
            Session.SetString("Cart", json);
        }
    }
}