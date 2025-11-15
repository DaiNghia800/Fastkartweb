using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Admin
{
    [Authorize(Policy = "NoCustomer")]
    [Route("/admin/orders")]
    public class OrderController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/Admin/Order/index.cshtml");
        }

        [HttpGet("detail")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Order/detail.cshtml");
        }
    }
}
