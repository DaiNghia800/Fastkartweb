using Fastkart.Models.Entities;
using Microsoft.AspNetCore.Mvc;


namespace Fastkart.Controllers.Client
{
    [Route("/cart")]
    public class CheckoutController : Controller
    {
        [HttpGet("checkout")]
        public IActionResult Checkout()
        {
            return View("~/Views/Order/Checkout.cshtml");
        }
    }
}