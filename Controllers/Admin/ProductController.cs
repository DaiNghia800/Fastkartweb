using Fastkart.Models.Entities;
using Fastkart.Services;
using Microsoft.AspNetCore.Mvc;
using Slugify;
using System.Text.Json;

namespace Fastkart.Controllers.Admin
{
    [Route("admin/product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            //filter
            string status = Request.Query["status"];
            if (string.IsNullOrEmpty(status))
            {
                status = null;
            }
            //end filter

            //sort
            string sortKey = Request.Query["sortKey"];
            string sortValue = Request.Query["sortValue"];
            bool descending = true;
            if(!string.IsNullOrEmpty(sortKey) && !string.IsNullOrEmpty(sortValue))
            {
                descending = sortValue == "desc";
            } else
            {
                descending = true;
                sortKey = "Position";
            }
            //end sort

            //pagination
            string pageStr = Request.Query["page"];
            int page = 1;
            int limitItem = 3;
            if (!string.IsNullOrEmpty(pageStr))
            {
                int.TryParse(pageStr, out page);
            }

            int skip = (page - 1) * limitItem;
            int totalProduct = _productService.CountProduct(status);
            int totalPage = (int)Math.Ceiling((double)totalProduct / limitItem);
            //pagination

            var listProduct = _productService.GetAllProducts(skip, limitItem, status, sortKey, descending);
            ViewData["Products"] = listProduct;
            ViewData["TotalPage"] = totalPage;
            ViewData["CurrentPage"] = page;
            return View("~/Views/Admin/Product/Index.cshtml");
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var listProductCategory = _productService.GetAllProductCategory();
            var listBrand = _productService.GetAllBrand();
            var listUnit = _productService.GetAllUnit();
            var listStockStatus = _productService.GetAllStockStatus();
            ViewData["productCategories"] = listProductCategory;
            ViewData["brands"] = listBrand;
            ViewData["units"] = listUnit;
            ViewData["stockStatus"] = listStockStatus;
            return View("~/Views/Admin/Product/Create.cshtml");
        }

        [HttpGet("subCategory")]
        public JsonResult GetSubCategories(int categoryId)
        {
            var subCategories = _productService.GetSubCategoryByCategory(categoryId);
            return Json(subCategories);
        }

        [HttpPost("create")]
        public IActionResult CreatePost([FromForm] Product product)
        {
            if (!ModelState.IsValid)
            {
                var listProductCategory = _productService.GetAllProductCategory();
                var listBrand = _productService.GetAllBrand();
                var listUnit = _productService.GetAllUnit();
                var listStockStatus = _productService.GetAllStockStatus();
                ViewData["productCategories"] = listProductCategory;
                ViewData["brands"] = listBrand;
                ViewData["units"] = listUnit;
                ViewData["stockStatus"] = listStockStatus;
                return View("~/Views/Admin/Product/Create.cshtml", product);
            }

            if (product.Position != null)
            {
                product.Position = (int)product.Position;
            } else
            {
                int count = _productService.CountProduct(null);
                product.Position = count + 1;
            }

            var slugHelper = new SlugHelper();
            product.Slug = slugHelper.GenerateSlug(product.ProductName);
            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = DateTime.Now;

            _productService.CreateProduct(product);
            return Redirect("/admin/product");
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var listProductCategory = _productService.GetAllProductCategory();
            var listBrand = _productService.GetAllBrand();
            var listUnit = _productService.GetAllUnit();
            var listStockStatus = _productService.GetAllStockStatus();
            var product = _productService.GetProduct(id);
            ViewData["productCategories"] = listProductCategory;
            ViewData["brands"] = listBrand;
            ViewData["units"] = listUnit;
            ViewData["stockStatus"] = listStockStatus;
            ViewData["product"] = product;
            return View("~/Views/Admin/Product/Edit.cshtml");
        }

        [HttpPost("edit/{id}")]
        public IActionResult EditPost([FromForm] Product data, int id)
        {
            if (!ModelState.IsValid)
            {
                var listProductCategory = _productService.GetAllProductCategory();
                var listBrand = _productService.GetAllBrand();
                var listUnit = _productService.GetAllUnit();
                var listStockStatus = _productService.GetAllStockStatus();
                var product = _productService.GetProduct(id);
                ViewData["productCategories"] = listProductCategory;
                ViewData["brands"] = listBrand;
                ViewData["units"] = listUnit;
                ViewData["stockStatus"] = listStockStatus;
                ViewData["product"] = product;
                return View("~/Views/Admin/Product/Edit.cshtml", data);
            }
            if (data.Position != null)
            {
                data.Position = (int)data.Position;
            }
            var slugHelper = new SlugHelper();
            data.Slug = slugHelper.GenerateSlug(data.ProductName);

            _productService.EditProduct(id, data);
            return Redirect($"/admin/product/edit/{id}");
        }

        [HttpPost("delete/{id}")]
        public JsonResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return Json(new {code = "success"});
        }

        [HttpPost("change-status/{id}")]
        public JsonResult ChangeStatus([FromBody] JsonElement data, int id)
        {
            string status = data.GetProperty("status").GetString();
            _productService.ChangeStatus(id, status);
            return Json(new { code = "success" });
        }

        [HttpPost("change-multi")]
        public JsonResult ChangeMulti([FromBody] JsonElement data)
        {
            string result = _productService.ChangeMulti(data);
            return Json(new {code = result });
        }

        [HttpPost("change-position")]
        public JsonResult ChangePosition([FromBody] JsonElement data)
        {
            _productService.ChangePosition(data);
            return Json(new { code = "success" });
        }

    }
}
