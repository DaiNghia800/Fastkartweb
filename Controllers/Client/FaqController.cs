using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Client
{
    public class FaqController : Controller
    {
        [Route("/faq")]
        public IActionResult Index()
        {
            return View("~/Views/FAQ/Index.cshtml");
        }
    }
}
