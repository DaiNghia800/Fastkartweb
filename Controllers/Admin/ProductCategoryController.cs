using Fastkart.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Fastkart.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Slugify;
using System.Text.Json;

namespace Fastkart.Controllers.Admin
{
    [Authorize(Policy = "NoCustomer")]
    [Route("/admin/product-category")]
    [Authorize]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
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
            if (!string.IsNullOrEmpty(sortKey) && !string.IsNullOrEmpty(sortValue))
            {
                descending = sortValue == "desc";
            }
            else
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
            int totalProduct = _productCategoryService.CountProduct(status);
            int totalPage = (int)Math.Ceiling((double)totalProduct / limitItem);
            //pagination
            var listProductCategory = _productCategoryService.GetAllProductCategory(skip, limitItem, status, sortKey, descending);
            ViewData["productCategories"] = listProductCategory;
            ViewData["TotalPage"] = totalPage;
            ViewData["CurrentPage"] = page;
            return View("~/Views/Admin/ProductCategory/Index.cshtml");
        }

        [HttpGet("create")]
        public IActionResult Create() {
            return View("~/Views/Admin/ProductCategory/Create.cshtml");
        }

        [HttpPost("create")]
        public IActionResult CreatePost([FromForm] ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/ProductCategory/Create.cshtml", productCategory);
            }

            if (productCategory.Position != null)
            {
                productCategory.Position = (int)productCategory.Position;
            }
            else
            {
                int count = _productCategoryService.CountProduct(null);
                productCategory.Position = count + 1;
            }

            var slugHelper = new SlugHelper();
            productCategory.Slug = slugHelper.GenerateSlug(productCategory.CategoryName);
            productCategory.CreatedAt = DateTime.Now;
            productCategory.UpdatedAt = DateTime.Now;
            _productCategoryService.CreateCategory(productCategory);

            return Redirect("/admin/product-category");
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var productCategory = _productCategoryService.GetProductCategory(id);
            ViewData["productCategory"] = productCategory;
            return View("~/Views/Admin/ProductCategory/Edit.cshtml");
        }

        [HttpPost("edit/{id}")]
        public IActionResult EditPost([FromForm] ProductCategory productCategory ,int id)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/ProductCategory/Edit.cshtml", productCategory);
            }

            if (productCategory.Position != null)
            {
                productCategory.Position = (int)productCategory.Position;
            }

            var slugHelper = new SlugHelper();
            productCategory.Slug = slugHelper.GenerateSlug(productCategory.CategoryName);
            _productCategoryService.EditCategory(productCategory, id);

            return Redirect($"/admin/product-category/edit/{id}");
        }

        [HttpPost("delete/{id}")]
        public JsonResult Delete(int id)
        {
            _productCategoryService.DeleteProduct(id);
            return Json(new { code = "success" });
        }

        [HttpPost("change-status/{id}")]
        public JsonResult ChangeStatus([FromBody] JsonElement data, int id)
        {
            string status = data.GetProperty("status").GetString();
            _productCategoryService.ChangeStatus(id, status);
            return Json(new { code = "success" });
        }

        [HttpPost("change-multi")]
        public JsonResult ChangeMulti([FromBody] JsonElement data)
        {
            string result = _productCategoryService.ChangeMulti(data);
            return Json(new { code = result });
        }

        [HttpPost("change-position")]
        public JsonResult ChangePosition([FromBody] JsonElement data)
        {
            _productCategoryService.ChangePosition(data);
            return Json(new { code = "success" });
        }
    }
}
