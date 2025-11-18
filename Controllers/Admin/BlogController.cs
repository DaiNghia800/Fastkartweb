using Microsoft.AspNetCore.Mvc;
using Fastkart.Models.Entities;
using Fastkart.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using Fastkart.Controllers.Admin;

namespace Fastkart.Controllers
{
    public class BlogController : Controller
    {
        // === THAY ĐỔI: Bỏ _context, dùng _blogService ===
        private readonly IBlogService _blogService;
        private readonly IUploadService _uploadService;

        // === THAY ĐỔI: Inject IBlogService ===
        public BlogController(IBlogService blogService, IUploadService uploadService)
        {
            _blogService = blogService;
            _uploadService = uploadService;
        }

        // ============================================
        // PHẦN PUBLIC (Không thay đổi nhiều, chỉ gọi Service)
        // ============================================

        [HttpGet("blog")]
        public IActionResult Index()
        {
            return View("~/Views/Blog/Index.cshtml");
        }

        [HttpGet("blog/{id}")]
        public IActionResult Detail(int id)
        {
            ViewData["BlogId"] = id;
            return View("~/Views/Blog/Detail.cshtml");
        }

        [HttpGet("blog/api/list")]
        public async Task<IActionResult> GetBlogsPublic()
        {
            // === SỬA: Gọi Service ===
            var blogs = await _blogService.GetBlogsPublicAsync();
            return Ok(blogs);
        }

        [HttpGet("blog/api/detail/{id}")]
        public async Task<IActionResult> GetBlogDetailPublic(int id)
        {
            // === SỬA: Gọi Service ===
            var blog = await _blogService.GetBlogDetailPublicAsync(id);
            if (blog == null)
            {
                return NotFound(new { message = "Blog not found" });
            }
            return Ok(blog);
        }

        [HttpGet("blog/api/categories")]
        public async Task<IActionResult> GetCategoriesPublic()
        {
            // === SỬA: Gọi Service ===
            var categories = await _blogService.GetCategoriesPublicAsync();
            return Ok(categories);
        }

        // ============================================
        // PHẦN ADMIN CATEGORIES (Gọi Service)
        // ============================================

        [HttpGet("api/BlogCategories")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<ActionResult<IEnumerable<BlogCategories>>> GetBlogCategories()
        {
            // === SỬA: Gọi Service ===
            var categories = await _blogService.GetCategoriesAdminAsync();
            return Ok(categories);
        }

        [HttpGet("api/BlogCategories/{id}")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<ActionResult<BlogCategories>> GetBlogCategory(int id)
        {
            // === SỬA: Gọi Service ===
            var category = await _blogService.GetCategoryAdminAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            return Ok(category);
        }

        [HttpPost("api/BlogCategories")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<ActionResult<BlogCategories>> CreateBlogCategory(CreateBlogCategoryDto createDto)
        {
            // === SỬA: Gọi Service ===
            var category = await _blogService.CreateCategoryAsync(createDto);
            return CreatedAtAction(nameof(GetBlogCategory), new { id = category.Uid }, category);
        }

        [HttpPut("api/BlogCategories/{id}")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<IActionResult> UpdateBlogCategory(int id, CreateBlogCategoryDto updateDto)
        {
            // === SỬA: Gọi Service ===
            var category = await _blogService.UpdateCategoryAsync(id, updateDto);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            return Ok(new { message = "Category updated successfully" });
        }

        [HttpDelete("api/BlogCategories/{id}")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<IActionResult> DeleteBlogCategory(int id)
        {
            try
            {
                // === SỬA: Gọi Service ===
                var success = await _blogService.DeleteCategoryAsync(id);
                if (!success)
                {
                    return NotFound(new { message = "Category not found" });
                }
                return Ok(new { message = "Category deleted successfully" });
            }
            catch (InvalidOperationException ex) // Bắt lỗi "Cannot delete"
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // ============================================
        // PHẦN ADMIN BLOGS (Gọi Service)
        // ============================================

        // --- View Routes (Không đổi) ---
        [HttpGet("admin/blog")]
        [Authorize(Policy = "NoCustomer")]
        public IActionResult IndexAdmin()
        {
            return View("~/Views/Admin/Blog/Index.cshtml");
        }

        [HttpGet("admin/blog/create")]
        [Authorize(Policy = "NoCustomer")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Blog/Create.cshtml");
        }

        [HttpGet("admin/blog/edit/{id}")]
        [Authorize(Policy = "NoCustomer")]
        public IActionResult Edit(int id)
        {
            ViewData["BlogId"] = id;
            return View("~/Views/Admin/Blog/Create.cshtml");
        }

        // --- API Routes ---
        [HttpPost("admin/blog/api/upload-image")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            // (Phần này giữ nguyên vì nó dùng IUploadService)
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file provided" });
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { message = "Invalid file type. Only images are allowed." });
            }
            if (file.Length > 5 * 1024 * 1024)
            {
                return BadRequest(new { message = "File size exceeds 5MB limit" });
            }
            try
            {
                var imageUrl = await _uploadService.UploadImageAsync(file);
                return Ok(new { imageUrl = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error uploading image: " + ex.Message });
            }
        }

        [HttpGet("admin/blog/api/list")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetBlogsAdmin()
        {
            // === SỬA: Gọi Service ===
            var blogs = await _blogService.GetBlogsAdminAsync();
            return Ok(blogs);
        }

        [HttpGet("admin/blog/api/detail/{id}")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<ActionResult<BlogPostDto>> GetBlogAdmin(int id)
        {
            // === SỬA: Gọi Service ===
            var blog = await _blogService.GetBlogAdminAsync(id);
            if (blog == null)
            {
                return NotFound(new { message = "Blog not found" });
            }
            return Ok(blog);
        }

        [HttpPost("admin/blog/api/create")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<ActionResult<BlogPosts>> CreateBlog([FromBody] CreateBlogDto createDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            try
            {
                // === SỬA: Gọi Service (Service đã bao gồm validation) ===
                var blog = await _blogService.CreateBlogAsync(createDto, currentUserId);
                return Ok(new { message = "Blog created successfully", uid = blog.Uid });
            }
            catch (ArgumentException ex) // Bắt lỗi "Invalid category"
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("admin/blog/api/update/{id}")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] CreateBlogDto updateDto)
        {
            // (Không cần check UserID ở đây vì [Authorize] đã làm rồi)
            try
            {
                // === SỬA: Gọi Service (Service đã bao gồm validation) ===
                var blog = await _blogService.UpdateBlogAsync(id, updateDto);
                if (blog == null)
                {
                    return NotFound(new { message = "Blog not found" });
                }
                return Ok(new { message = "Blog updated successfully" });
            }
            catch (ArgumentException ex) // Bắt lỗi "Invalid category"
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("admin/blog/api/delete/{id}")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            // === SỬA: Gọi Service ===
            var success = await _blogService.DeleteBlogAsync(id);
            if (!success)
            {
                return NotFound(new { message = "Blog not found" });
            }
            return Ok(new { message = "Blog deleted successfully" });
        }

        [HttpGet("admin/blog/api/categories")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<ActionResult<IEnumerable<BlogCategories>>> GetCategoriesAdmin()
        {
            // === SỬA: Gọi Service ===
            var categories = await _blogService.GetCategoriesAdminAsync();
            return Ok(categories);
        }

        [HttpGet("admin/blog/api/authors")] // <-- ĐÂY LÀ ROUTE MÀ JS CẦN
        [Authorize(Policy = "NoCustomer")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetBlogAuthors()
        {
            // === SỬA: Gọi Service ===
            var authors = await _blogService.GetBlogAuthorsAsync();
            return Ok(authors);
        }

        [HttpGet("admin/blog/api/current-user")]
        [Authorize(Policy = "NoCustomer")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            // === SỬA: Gọi Service ===
            var user = await _blogService.GetCurrentUserAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(user);
        }

        // === BỎ: Các hàm Helper (đã chuyển sang Service) ===
        // private bool CategoryExists(int id) { ... }
        // private bool BlogExists(int id) { ... }
    }


    
}