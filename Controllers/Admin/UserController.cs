using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Admin
{
    [Route("/admin/user")]
    public class UserController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/Admin/User/index.cshtml");
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/User/create.cshtml");
        }
    }
}
