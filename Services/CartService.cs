using Fastkart.Models.Entities;
using Fastkart.Models.EF;         
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

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


        public List<CartItemViewModel> GetCartItems()
        {
            var json = Session.GetString("Cart");
            if (string.IsNullOrEmpty(json))
            {
                return new List<CartItemViewModel>();
            }
            return JsonConvert.DeserializeObject<List<CartItemViewModel>>(json);
        }

        private void SaveCartItems(List<CartItemViewModel> items)
        {
            var json = JsonConvert.SerializeObject(items);
            Session.SetString("Cart", json);
        }

        public void AddToCart(int productId, int quantity = 1)
        {
            var cartItems = GetCartItems();
            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var product = _context.Product.FirstOrDefault(p => p.Uid == productId);
                if (product != null)
                {
                    var newItem = new CartItemViewModel
                    {
                        ProductId = product.Uid,
                        ProductName = product.ProductName,
                        Image = product.Thumbnail,
                        Price = (long)product.Price,
                        Quantity = quantity
                    };
                    cartItems.Add(newItem);
                }
            }

            SaveCartItems(cartItems);
        }

        public void RemoveFromCart(int productId)
        {
            var cartItems = GetCartItems();
            var itemToRemove = cartItems.FirstOrDefault(item => item.ProductId == productId);

            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                SaveCartItems(cartItems);
            }
        }

        public void ClearCart()
        {
            Session.Remove("Cart");
        }

        public long GetSubtotal()
        {
            return GetCartItems().Sum(item => item.Total);
        }
    }
}