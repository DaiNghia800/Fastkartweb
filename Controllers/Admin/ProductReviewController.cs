using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Admin
{
    [Route("/admin/product-review")]
    public class ProductReviewController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/Admin/Product/review.cshtml");
        }
    }
}
