using Fastkart.Models.Entities;
using Fastkart.Services;
using Microsoft.AspNetCore.Mvc;
using Slugify;
using System.Text.Json;

namespace Fastkart.Controllers.Admin
{
    [Route("/admin/sub-category")]
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
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

            //pagination
            string pageStr = Request.Query["page"];
            int page = 1;
            int limitItem = 5;
            if (!string.IsNullOrEmpty(pageStr))
            {
                int.TryParse(pageStr, out page);
            }

            int skip = (page - 1) * limitItem;
            int totalProduct = _subCategoryService.CountSubCategory(status);
            int totalPage = (int)Math.Ceiling((double)totalProduct / limitItem);
            //pagination

            var listSubCategory = _subCategoryService.GetAllSubCategory(skip, limitItem, status);
            ViewData["productSubCategories"] = listSubCategory;
            ViewData["TotalPage"] = totalPage;
            ViewData["CurrentPage"] = page;
            return View("~/Views/Admin/SubCategory/Index.cshtml");
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var listProductCategory = _subCategoryService.GetAllProductCategory();
            ViewData["productCategories"] = listProductCategory;
            return View("~/Views/Admin/SubCategory/Create.cshtml");
        }

        [HttpPost("create")]
        public IActionResult CreatePost([FromForm] ProductSubCategory productSubCategory)
        {
            if (!ModelState.IsValid)
            {
                var listProductCategory = _subCategoryService.GetAllProductCategory();
                ViewData["productCategories"] = listProductCategory;
                return View("~/Views/Admin/SubCategory/Create.cshtml", productSubCategory);
            }

            var slugHelper = new SlugHelper();
            productSubCategory.Slug = slugHelper.GenerateSlug(productSubCategory.SubCategoryName);
            productSubCategory.CreatedAt = DateTime.Now;
            productSubCategory.UpdatedAt = DateTime.Now;
            _subCategoryService.CreateSubCategory(productSubCategory);

            return Redirect("/admin/sub-category");
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var listProductCategory = _subCategoryService.GetAllProductCategory();
            var productSubCategory = _subCategoryService.GetSubCategory(id);
            ViewData["productCategories"] = listProductCategory;
            ViewData["productSubCategory"] = productSubCategory;
            return View("~/Views/Admin/SubCategory/Edit.cshtml");
        }

        [HttpPost("edit/{id}")]
        public IActionResult EditPost([FromForm] ProductSubCategory productSubCategory, int id)
        {
            if (!ModelState.IsValid)
            {
                var listProductCategory = _subCategoryService.GetAllProductCategory();
                ViewData["productCategories"] = listProductCategory;
                return View("~/Views/Admin/SubCategory/Edit.cshtml", productSubCategory);
            }

            var slugHelper = new SlugHelper();
            productSubCategory.Slug = slugHelper.GenerateSlug(productSubCategory.SubCategoryName);
            _subCategoryService.EditSubCategory(productSubCategory, id);

            return Redirect($"/admin/sub-category/edit/{id}");
        }

        [HttpPost("delete/{id}")]
        public JsonResult Delete(int id)
        {
            _subCategoryService.DeleteSubCategory(id);
            return Json(new { code = "success" });
        }

        [HttpPost("change-status/{id}")]
        public JsonResult ChangeStatus([FromBody] JsonElement data, int id)
        {
            string status = data.GetProperty("status").GetString();
            _subCategoryService.ChangeStatus(id, status);
            return Json(new { code = "success" });
        }

        [HttpPost("change-multi")]
        public JsonResult ChangeMulti([FromBody] JsonElement data)
        {
            string result = _subCategoryService.ChangeMulti(data);
            return Json(new { code = result });
        }
    }
}
