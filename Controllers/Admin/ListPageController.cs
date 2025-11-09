using Fastkart.Models.Entities;
using Fastkart.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions; // Thêm thư viện này

namespace Fastkart.Controllers.Admin
{
    [Route("/admin/list-page")]
    public class ListPageController : Controller
    {
        private readonly IPageService _pageService;

        public ListPageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<Pages> pageList = _pageService.GetAllPages();
            return View("~/Views/Admin/ListPage/index.cshtml", pageList);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/ListPage/ListPageCreate.cshtml");
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken] // <-- Nên thêm để bảo mật
        public IActionResult Create(Pages newPage)
        {
            try
            {
                // Gán các giá trị mà người dùng không nhập
                // GÁN CỨNG ĐỂ TEST - Yêu cầu User Uid = 1 tồn tại
                newPage.AuthorUid = 1;
                newPage.CreatedBy = "admin";
                newPage.UpdatedBy = "admin";
                newPage.CreatedAt = DateTime.Now;
                newPage.UpdatedAt = DateTime.Now;
                newPage.PublishedAt = DateTime.Now;
                newPage.Deleted = false;
                newPage.Status = "Published";
                newPage.Slug = GenerateSlug(newPage.Title); // Tạo Slug

                // XÓA VALIDATION cho các trường ta vừa tự gán
                ModelState.Remove(nameof(newPage.Author));
                ModelState.Remove(nameof(newPage.AuthorUid));
                ModelState.Remove(nameof(newPage.Slug));
                // Xóa thêm các trường khác nếu chúng bị validate
                ModelState.Remove(nameof(newPage.CreatedBy));
                ModelState.Remove(nameof(newPage.UpdatedBy));
                // ...

                // Giờ ta có thể kiểm tra validation cho các trường còn lại (Title, Content)
                if (ModelState.IsValid)
                {
                    Console.WriteLine("=================================");
                    Console.WriteLine($"📝 Chuẩn bị lưu Page:");
                    Console.WriteLine($"  Title: {newPage.Title}");
                    Console.WriteLine($"  AuthorUid: {newPage.AuthorUid}");
                    Console.WriteLine("=================================");

                    _pageService.CreatePage(newPage);
                    Console.WriteLine("✅ LƯU THÀNH CÔNG!");
                    return RedirectToAction("Index");
                }

                // Nếu IsValid = false, các lỗi (ví dụ Title rỗng) sẽ tự động hiển thị
                ViewBag.ErrorMessage = "Dữ liệu nhập không hợp lệ. Vui lòng kiểm tra lại.";
                return View("~/Views/Admin/ListPage/ListPageCreate.cshtml", newPage);
            }
            catch (DbUpdateException dbEx)
            {
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                Console.WriteLine($"❌ LỖI DATABASE: {innerMessage}");
                ViewBag.ErrorMessage = $"Lỗi khi lưu vào database: {innerMessage}";
                return View("~/Views/Admin/ListPage/ListPageCreate.cshtml", newPage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ LỖI: {ex.Message}");
                ViewBag.ErrorMessage = $"Lỗi: {ex.Message}";
                return View("~/Views/Admin/ListPage/ListPageCreate.cshtml", newPage);
            }
        }

        // ... (Action Delete của bạn) ...

        /// <summary>
        /// Hàm tạo Slug tốt hơn, hỗ trợ tiếng Việt
        /// </summary>
        private string GenerateSlug(string title)
        {
            if (string.IsNullOrEmpty(title)) return Guid.NewGuid().ToString();

            title = title.ToLower().Trim();

            // Bỏ dấu tiếng Việt
            title = Regex.Replace(title, "áàạảãâấầậẩẫăắằặẳẵ", "a");
            title = Regex.Replace(title, "éèẹẻẽêếềệểễ", "e");
            title = Regex.Replace(title, "íìịỉĩ", "i");
            title = Regex.Replace(title, "óòọỏõôốồộổỗơớờợởỡ", "o");
            title = Regex.Replace(title, "úùụủũưứừựửữ", "u");
            title = Regex.Replace(title, "ýỳỵỷỹ", "y");
            title = Regex.Replace(title, "đ", "d");

            // Xóa các ký tự đặc biệt
            title = Regex.Replace(title, @"[^a-z0-9\s-]", "");

            // Thay thế khoảng trắng bằng gạch nối
            title = Regex.Replace(title, @"\s+", "-").Trim('-');

            // Đảm bảo không có 2 dấu gạch nối liền nhau
            title = Regex.Replace(title, @"-+", "-");

            return title;
        }
    }
}