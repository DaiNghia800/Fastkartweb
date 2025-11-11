using Fastkart.Models.Entities;
using Fastkart.Models.EF;
using Fastkart.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Fastkart.Controllers.Admin
{
    [Route("/admin/list-page")]
    public class ListPageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly ApplicationDbContext _context;

        public ListPageController(IPageService pageService, ApplicationDbContext context)
        {
            _pageService = pageService;
            _context = context;
        }

        // ============================================
        // INDEX - Danh sách pages
        // ============================================
        [HttpGet("")]
        public IActionResult Index()
        {
            try
            {
                List<Pages> pageList = _pageService.GetAllPages();
                return View("~/Views/Admin/ListPage/index.cshtml", pageList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading pages: {ex.Message}");
                ViewBag.ErrorMessage = "Unable to load pages. Please try again.";
                return View("~/Views/Admin/ListPage/index.cshtml", new List<Pages>());
            }
        }

        // ============================================
        // CREATE - GET: Hiển thị form tạo mới
        // ============================================
        [HttpGet("create")]
        public IActionResult Create()
        {
            // Load danh sách authors (users)
            LoadAuthors();
            return View("~/Views/Admin/ListPage/ListPageCreate.cshtml", new Pages());
        }

        // ============================================
        // CREATE - POST: Xử lý tạo mới
        // ============================================
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pages newPage)
        {
            try
            {
                // Xóa validation cho các trường tự động
                RemoveAutoFieldsFromValidation();

                // Validate Author
                if (newPage.AuthorUid <= 0)
                {
                    ModelState.AddModelError("AuthorUid", "Please select an author");
                }

                if (!ModelState.IsValid)
                {
                    LoadAuthors();
                    ViewBag.ErrorMessage = "Please check all required fields.";
                    return View("~/Views/Admin/ListPage/ListPageCreate.cshtml", newPage);
                }

                // Gán giá trị tự động
                newPage.Slug = GenerateSlug(newPage.Title);
                newPage.CreatedAt = DateTime.Now;
                newPage.UpdatedAt = DateTime.Now;
                newPage.CreatedBy = GetCurrentUserName(); // TODO: Lấy từ session/auth
                newPage.UpdatedBy = GetCurrentUserName();
                newPage.Deleted = false;

                // Set PublishedAt nếu status = Published
                if (newPage.Status == "Published")
                {
                    newPage.PublishedAt = DateTime.Now;
                }

                _pageService.CreatePage(newPage);

                Console.WriteLine($"✅ Page created successfully: {newPage.Title}");
                TempData["SuccessMessage"] = $"Page '{newPage.Title}' created successfully!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException dbEx)
            {
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                Console.WriteLine($"❌ Database error: {innerMessage}");

                LoadAuthors();
                ViewBag.ErrorMessage = "Database error. Please check if the author exists.";
                return View("~/Views/Admin/ListPage/ListPageCreate.cshtml", newPage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");

                LoadAuthors();
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("~/Views/Admin/ListPage/ListPageCreate.cshtml", newPage);
            }
        }

        // ============================================
        // EDIT - GET: Hiển thị form chỉnh sửa
        // ============================================
        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            try
            {
                var page = _pageService.GetPageById(id);

                if (page == null)
                {
                    TempData["ErrorMessage"] = "Page not found.";
                    return RedirectToAction("Index");
                }

                LoadAuthors();
                return View("~/Views/Admin/ListPage/ListPageCreate.cshtml", page);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading page: {ex.Message}");
                TempData["ErrorMessage"] = "Unable to load page for editing.";
                return RedirectToAction("Index");
            }
        }

        // ============================================
        // EDIT - POST: Xử lý cập nhật
        // ============================================
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Pages updatedPage)
        {
            try
            {
                if (id != updatedPage.Uid)
                {
                    return BadRequest("Page ID mismatch");
                }

                var existingPage = _pageService.GetPageById(id);
                if (existingPage == null)
                {
                    TempData["ErrorMessage"] = "Page not found.";
                    return RedirectToAction("Index");
                }

                // Xóa validation
                RemoveAutoFieldsFromValidation();

                if (!ModelState.IsValid)
                {
                    LoadAuthors();
                    ViewBag.ErrorMessage = "Please check all required fields.";
                    return View("~/Views/Admin/ListPage/ListPageCreate.cshtml", updatedPage);
                }

                // Cập nhật thông tin
                existingPage.Title = updatedPage.Title;
                existingPage.Content = updatedPage.Content;
                existingPage.Status = updatedPage.Status;
                existingPage.AuthorUid = updatedPage.AuthorUid;
                existingPage.Slug = GenerateSlug(updatedPage.Title);
                existingPage.UpdatedAt = DateTime.Now;
                existingPage.UpdatedBy = GetCurrentUserName();

                // Cập nhật PublishedAt nếu chuyển sang Published
                if (updatedPage.Status == "Published" && existingPage.PublishedAt == null)
                {
                    existingPage.PublishedAt = DateTime.Now;
                }

                _pageService.UpdatePage(existingPage);

                Console.WriteLine($"✅ Page updated: {existingPage.Title}");
                TempData["SuccessMessage"] = $"Page '{existingPage.Title}' updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error updating page: {ex.Message}");

                LoadAuthors();
                ViewBag.ErrorMessage = $"Error updating page: {ex.Message}";
                return View("~/Views/Admin/ListPage/ListPageCreate.cshtml", updatedPage);
            }
        }

        // ============================================
        // DELETE - Soft delete (set Deleted = true)
        // ============================================
        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var page = _pageService.GetPageById(id);

                if (page == null)
                {
                    TempData["ErrorMessage"] = "Page not found.";
                    return RedirectToAction("Index");
                }

                _pageService.DeletePage(id);

                Console.WriteLine($"✅ Page deleted: {page.Title}");
                TempData["SuccessMessage"] = $"Page '{page.Title}' deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error deleting page: {ex.Message}");
                TempData["ErrorMessage"] = "Unable to delete page.";
                return RedirectToAction("Index");
            }
        }

        // ============================================
        // HELPER METHODS
        // ============================================

        /// <summary>
        /// Load danh sách authors vào ViewBag
        /// </summary>
        private void LoadAuthors()
        {
            try
            {
                ViewBag.Authors = _context.Users
                    .Where(u => u.Deleted == false)
                    .Select(u => new { u.Uid, u.FullName })
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading authors: {ex.Message}");
                ViewBag.Authors = new List<dynamic>();
            }
        }

        /// <summary>
        /// Xóa validation cho các trường tự động
        /// </summary>
        private void RemoveAutoFieldsFromValidation()
        {
            ModelState.Remove("Author");
            ModelState.Remove("Slug");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("UpdatedAt");
            ModelState.Remove("CreatedBy");
            ModelState.Remove("UpdatedBy");
            ModelState.Remove("Deleted");
            ModelState.Remove("PublishedAt");
        }

        /// <summary>
        /// Tạo slug từ title (hỗ trợ tiếng Việt)
        /// </summary>
        private string GenerateSlug(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Guid.NewGuid().ToString();

            title = title.ToLower().Trim();

            // Bỏ dấu tiếng Việt
            title = Regex.Replace(title, "[áàạảãâấầậẩẫăắằặẳẵ]", "a");
            title = Regex.Replace(title, "[éèẹẻẽêếềệểễ]", "e");
            title = Regex.Replace(title, "[íìịỉĩ]", "i");
            title = Regex.Replace(title, "[óòọỏõôốồộổỗơớờợởỡ]", "o");
            title = Regex.Replace(title, "[úùụủũưứừựửữ]", "u");
            title = Regex.Replace(title, "[ýỳỵỷỹ]", "y");
            title = Regex.Replace(title, "đ", "d");

            // Xóa ký tự đặc biệt
            title = Regex.Replace(title, @"[^a-z0-9\s-]", "");

            // Thay khoảng trắng = dấu gạch nối
            title = Regex.Replace(title, @"\s+", "-").Trim('-');

            // Xóa dấu gạch nối liên tiếp
            title = Regex.Replace(title, @"-+", "-");

            return title;
        }

        /// <summary>
        /// Lấy username hiện tại
        /// TODO: Implement authentication và lấy từ User.Identity.Name
        /// </summary>
        private string GetCurrentUserName()
        {
            // Tạm thời return "admin", sau này implement authentication
            return User?.Identity?.Name ?? "admin";
        }
    }
}