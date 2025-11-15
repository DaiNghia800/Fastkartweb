using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Admin
{
    [Route("/admin/dashboard")]
    [Authorize(Policy = "NoCustomer")]
    public class DashboardController : Controller
    {
        [HttpGet("")]
        public IActionResult Dashboard()
        {
            return View("~/Views/Admin/Dashboard.cshtml");
        }
    }
}
