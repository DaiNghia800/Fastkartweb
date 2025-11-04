using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Admin
{
    [Route("/admin/list-page")]
    public class ListPageController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/Admin/ListPage/index.cshtml");
        }
    }
}
