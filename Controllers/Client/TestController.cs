using Fastkart.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers
{
    // Controller này chỉ dùng để TEST
    public class TestController : Controller
    {
        private readonly CartService _cartService;

        public TestController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("test/addall")]
        public IActionResult AddAllToCart()
        {
            _cartService.AddToCart(1, 1); 
            _cartService.AddToCart(2, 2); 
            _cartService.AddToCart(3, 1); 

            return RedirectToAction("Index", "Cart");
        }

        [HttpGet("test/clear")]
        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            return RedirectToAction("Index", "Cart");
        }
    }
}