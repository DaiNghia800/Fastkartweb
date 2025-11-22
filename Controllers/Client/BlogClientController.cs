using Microsoft.AspNetCore.Mvc;

namespace Fastkart.Controllers.Client
{
    [Route("/blogs")]
    public class BlogClientController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/Blog/blog-list.cshtml");
        }
        // Trong Fastkart.Controllers.Client/BlogClientController.cs
        [HttpGet("detail")]
        public IActionResult BlogDetails([FromQuery] string id) // THÊM tham số ID
        {
            ViewData["BlogId"] = id; // Truyền ID sang View để JS có thể dùng
            return View("~/Views/Blog/blog-detail.cshtml");
        }
    }
}
