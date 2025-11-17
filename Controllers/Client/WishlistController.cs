using Fastkart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fastkart.Controllers.Client
{
    [Route("wishlist")]
    [Authorize] 
    public class WishlistController : Controller
    {
        private readonly WishlistService _wishlistService;

        public WishlistController(WishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdStr, out int userId);

            var products = await _wishlistService.GetUserWishlist(userId);
            return View("~/Views/Wishlist/Index.cshtml", products);
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle(int productId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Unauthorized(new { message = "Please log in to use this feature." });
            }
            int.TryParse(userIdStr, out int userId);

            bool isLiked = await _wishlistService.ToggleWishlist(userId, productId);
            int newCount = await _wishlistService.GetCount(userId);

            return Ok(new { success = true, isLiked = isLiked, totalItems = newCount });
        }
    }
}