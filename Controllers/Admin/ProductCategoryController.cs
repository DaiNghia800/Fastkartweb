using Fastkart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly IFastkartService _fastkartService;

        public ProductCategoryController(IFastkartService fastkartService)
        {
            _fastkartService = fastkartService;
        }

        [Route("/admin/product-category")]
        public IActionResult Index()
        {
            var listProductCategory = _fastkartService.GetAllProductCategory();
            ViewData["productCategories"] = listProductCategory;
            return View("~/Views/Admin/ProductCategory/Index.cshtml");
        }

        [Route("/admin/product-category/create")]
        public IActionResult Create() {
            return View("~/Views/Admin/ProductCategory/Create.cshtml");
        }
    }
}
