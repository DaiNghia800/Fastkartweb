using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Admin
{
    [Route("/admin/settings/profile")]
    public class SettingController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/Admin/Setting/index.cshtml");
        }
    }
}
