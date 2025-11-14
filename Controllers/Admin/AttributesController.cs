using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Admin
{
    [Authorize(Policy = "NoCustomer")]
    public class AttributesController : Controller
    {
        [Route("/admin/attributes")]
        public IActionResult Index()
        {
            return View("~/Views/Admin/Attributes/Index.cshtml");
        }

        [Route("/admin/attributes/create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Attributes/Create.cshtml");
        }
    }
}
