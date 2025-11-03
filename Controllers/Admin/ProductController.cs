using Fastkart.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Admin
{
    [Route("admin/product")]
    public class ProductController : Controller
    {
        private readonly IFastkartService _fastkartService;

        public ProductController(IFastkartService fastkartService)
        {
            _fastkartService = fastkartService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {        
            var listProduct = _fastkartService.GetAllProducts();
            ViewData["Products"] = listProduct;
            return View("~/Views/Admin/Product/Index.cshtml");
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var listProductCategory = _fastkartService.GetAllProductCategory();
            var listBrand = _fastkartService.GetAllBrand();
            var listUnit = _fastkartService.GetAllUnit();
            var listOptionName = _fastkartService.GetAllOptionName();
            var listStockStatus = _fastkartService.GetAllStockStatus();
            ViewData["productCategories"] = listProductCategory;
            ViewData["brands"] = listBrand;
            ViewData["units"] = listUnit;
            ViewData["optionNames"] = listOptionName;
            ViewData["stockStatus"] = listStockStatus;
            return View("~/Views/Admin/Product/Create.cshtml");
        }

        [HttpGet("subCategory")]
        public JsonResult GetSubCategories(int categoryId)
        {
            var subCategories = _fastkartService.GetSubCategoryByCategory(categoryId);
            return Json(subCategories);
        }

        [HttpGet("optionValue/{optionNameId}")]
        public JsonResult GetOptionValues(int optionNameId)
        {
            var optionValues = _fastkartService.GetOptionValues(optionNameId);
            return Json(optionValues);
        }
    }
}
