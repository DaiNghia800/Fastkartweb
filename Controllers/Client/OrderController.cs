using Fastkart.Models.EF;      
using Fastkart.Models.Entities; 
using Fastkart.Services;       
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Fastkart.Controllers
{
    [Route("order")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;

        public OrderController(ApplicationDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
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
                return BadRequest("Phương thức thanh toán không hợp lệ.");
            }

            var cartItems = _cartService.GetCartItems();
            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction("Index", "Cart"); 
            }

            long subtotal = _cartService.GetSubtotal();
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
            }

            _context.Order.Add(newOrder);
            await _context.SaveChangesAsync();

            _cartService.ClearCart();

            var createdOrder = await _context.Order
                .Include(o => o.OrderItems) 
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Uid == newOrder.Uid);

            ViewBag.Result = "Đặt hàng (COD) thành công!";
            ViewBag.Message = $"Đơn hàng của bạn sẽ sớm được giao.";
            ViewBag.OrderId = newOrder.Uid.ToString();
            ViewBag.Amount = newOrder.TotalAmount;

            return View("~/Views/Payment/Result.cshtml", createdOrder);
        }
    }
}