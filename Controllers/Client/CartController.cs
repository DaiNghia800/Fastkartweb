using Microsoft.AspNetCore.Mvc;
using Fastkart.Services;

namespace Fastkart.Controllers.Client
{
    [Route("/cart")]
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cartItems = await _cartService.GetCartItemsAsync();
            ViewBag.Subtotal = await _cartService.GetSubtotalAsync();

            return View("~/Views/Cart/Index.cshtml", cartItems);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            try
            {
                await _cartService.AddToCartAsync(productId, quantity);

                var cartItems = await _cartService.GetCartItemsAsync();
                int totalItems = cartItems.Sum(item => item.Quantity);

                return Ok(new
                {
                    success = true,
                    message = "Thêm thành công",
                    totalItems = totalItems
                });
            }
            catch (Exception ex)
            {
                // Ghi log lỗi tại đây nếu cần
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("remove")]
        public async Task<IActionResult> Remove(int productId)
        {
            await _cartService.RemoveFromCartAsync(productId);
            return RedirectToAction("Index");
        }
    }
}