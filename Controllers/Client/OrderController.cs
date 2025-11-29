using Fastkart.Models.EF;      
using Fastkart.Models.Entities; 
using Fastkart.Models.ViewModels;
using Fastkart.Services;       
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fastkart.Controllers
{
    [Route("order")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;
        private readonly EmailService _emailService;

        public OrderController(ApplicationDbContext context, CartService cartService, EmailService emailService)
        {
            _context = context;
            _cartService = cartService;
            _emailService = emailService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitOrder([FromForm] CheckoutFormViewModel formData)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }
            if (formData.PaymentMethod != "COD")
            {
                return BadRequest("Invalid payment method.");
            }

            var cartItems = await _cartService.GetCartItemsAsync();
            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            long subtotal = await _cartService.GetSubtotalAsync();
            long shippingFee = 25000;
            long couponDiscount = 10000;
            long finalTotal = subtotal + shippingFee - couponDiscount;

            var newOrder = new Order
            {
                OrderDate = DateTime.Now,
                Status = "Pending_COD",
                TotalAmount = (decimal)finalTotal,
                PaymentMethod = formData.PaymentMethod,
                ShippingAddress = formData.AddressId,
                UserUid = userId
            };

            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    Order = newOrder,
                    ProductUid = item.ProductId,
                    Quantity = item.Quantity,
                    PriceAtPurchase = item.Price
                };
                _context.OrderItem.Add(orderItem);

                var product = await _context.Product.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;
                }
            }

            _context.Order.Add(newOrder);
            await _context.SaveChangesAsync();
            var createdOrder = await _context.Order
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .Include(o => o.User) // <--- QUAN TRỌNG: Phải có dòng này để lấy Email
                .FirstOrDefaultAsync(o => o.Uid == newOrder.Uid);

            // 3. Gửi mail
            if (createdOrder != null)
            {
                _ = _emailService.SendOrderConfirmationAsync(createdOrder);
            }
            await _cartService.ClearCartAsync();

            ViewBag.Result = "(COD) Order placed successfully!";
            ViewBag.Message = $"Your order will be delivered soon!";
            ViewBag.OrderId = newOrder.Uid.ToString();
            ViewBag.Amount = newOrder.TotalAmount;

            return View("~/Views/Payment/Result.cshtml", createdOrder);
        }

        [HttpGet("my-orders")]
        [Authorize] 
        public async Task<IActionResult> MyOrders()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            //pagination
            string pageStr = Request.Query["page"];
            int page = 1;
            int limitItem = 10;
            if (!string.IsNullOrEmpty(pageStr))
            {
                int.TryParse(pageStr, out page);
            }

            int skip = (page - 1) * limitItem;
            int totalProduct = _context.Order.Count();
            int totalPage = (int)Math.Ceiling((double)totalProduct / limitItem);
            //pagination

            var orders = await _context.Order
                .Where(o => o.UserUid == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .Skip(skip)
                .Take(limitItem)
                .ToListAsync();

            var viewModels = orders.Select(o => {
                string displayProductName = "Order has no items";

                if (o.OrderItems.Any())
                {
                    var firstItemName = o.OrderItems.First().Product?.ProductName ?? "Product";
                    int otherCount = o.OrderItems.Count - 1;

                    if (otherCount > 0)
                        displayProductName = $"{firstItemName} (+{otherCount} others)";
                    else
                        displayProductName = firstItemName;
                }

                return new OrderHistoryViewModel
                {
                    Uid = o.Uid,
                    OrderCode = $"#{o.Uid}",
                    ProductName = displayProductName,
                    Status = o.Status,
                    TotalPrice = o.TotalAmount,
                    OrderDate = o.OrderDate
                };
            }).ToList();
            ViewData["TotalPage"] = totalPage;
            ViewData["CurrentPage"] = page;

            return View("~/Views/Order/MyOrders.cshtml", viewModels);
        }

        [HttpGet("details/{id}")]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            // 1. Lấy User ID hiện tại
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Tìm đơn hàng theo ID và UserID (Bảo mật)
            var order = await _context.Order
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Uid == id && o.UserUid == userId);

            if (order == null)
            {
                return NotFound(); // Hoặc Redirect về trang MyOrders
            }

            // 3. Trả về View với Model là đơn hàng tìm được
            return View("~/Views/Order/Details.cshtml", order);
        }
    }
}