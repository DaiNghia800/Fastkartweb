using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Client
{
    public class MainController : Controller
    {
        [Authorize(Roles = "Customer")]
        [Route("/home")]
        public IActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
