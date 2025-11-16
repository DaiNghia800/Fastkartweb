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
        public IActionResult Index()
        {
            var cartItems = _cartService.GetCartItems();

            ViewBag.Subtotal = _cartService.GetSubtotal();

            return View("~/Views/Cart/Index.cshtml", cartItems);
        }

        [HttpPost("add")]
        public IActionResult Add(int productId, int quantity = 1)
        {
            try
            {
                _cartService.AddToCart(productId, quantity);

                var cartItems = _cartService.GetCartItems();
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
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet("remove")]
        public IActionResult Remove(int productId)
        {
            _cartService.RemoveFromCart(productId);

            return RedirectToAction("Index");
        }
    }
}
