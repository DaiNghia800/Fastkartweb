using Fastkart.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Client
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("product/detail/{slug}")]
        public IActionResult Detail(string slug)
        {
            var product =  _productService.GetProduct(slug);
            var productRelated = _productService.GetProductBySubCategory(product.Uid ,product.SubCategoryUid);
            ViewData["productRelated"] = productRelated;
            ViewData["product"] = product;
            return View();
        }
    }
}
