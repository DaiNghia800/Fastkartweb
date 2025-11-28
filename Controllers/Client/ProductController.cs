using Fastkart.Services;
using Fastkart.Services.IServices;
using Microsoft.AspNetCore.Mvc;
namespace Fastkart.Controllers.Client
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly WishlistService _wishlistService;

        public ProductController(IProductService productService, WishlistService wishlistService)
        {
            _productService = productService;
            _wishlistService = wishlistService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("product/detail/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var product =  _productService.GetProduct(slug);
            var productRelated = _productService.GetProductBySubCategory(product.Uid ,product.SubCategoryUid);
            var wishlistIds = new List<int>();
            if (User.Identity.IsAuthenticated)
            {
                var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdStr, out int userId))
                {
                    wishlistIds = await _wishlistService.GetUserWishlistProductIds(userId);
                }
            }
            ViewBag.LikedProductIds = wishlistIds;
            ViewData["productRelated"] = productRelated;
            ViewData["product"] = product;
            return View();
        }
    }
}
