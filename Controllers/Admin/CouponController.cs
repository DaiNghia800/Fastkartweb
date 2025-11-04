using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Admin
{
    [Route("/admin/coupons")]
    public class CouponController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/Admin/Coupon/index.cshtml");
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Coupon/create.cshtml");
        }
    }
}
