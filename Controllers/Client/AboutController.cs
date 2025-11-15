using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Client
{
    [Route("/about-us")]
    public class AboutController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/About/Index.cshtml");
        }
    }
}
