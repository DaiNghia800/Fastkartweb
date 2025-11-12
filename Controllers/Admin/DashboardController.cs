using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Admin
{
    public class DashboardController : Controller
    {
        [Route("/admin/dashboard")]
        public IActionResult Dashboard()
        {
            return View("~/Views/Admin/Dashboard.cshtml");
        }
    }
}
