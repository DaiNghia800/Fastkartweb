using Fastkart.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Client
{
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("product-category/detail/{slug}")]
        public IActionResult Detail(string slug)
        {
            var category = _productCategoryService.GetProductCategory(slug);
            ViewData["category"] = category;
            return View();
        }
    }
}
