using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Client
{
    [Route("/cart")]
    public class CartController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/Cart/Index.cshtml");
        }
    }
}
