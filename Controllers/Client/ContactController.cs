using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Client
{
    public class ContactController : Controller
    {
        [Route("/contact-us")]
        public IActionResult Index()
        {
            return View("~/Views/Contact/Index.cshtml");
        }
    }
}
