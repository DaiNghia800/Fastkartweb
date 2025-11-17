using Fastkart.Models;
using Fastkart.Models.Entities;
using Fastkart.Services;
using Fastkart.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Fastkart.Controllers.Client
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly WishlistService _wishlistService;

        public HomeController(IHomeService homeService, WishlistService wishlistService)
        {
            _homeService = homeService;
            _wishlistService = wishlistService;
        }

        public async Task<IActionResult> Index()
        {

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
            var listCategory = _homeService.GetAllCategory();
            var listProduct = _homeService.GetAllProduct();
            var listNewProduct = _homeService.GetNewProduct();
            var listFeature = _homeService.GetFeatureProduct();
            ViewData["products"] = listProduct;
            ViewData["categories"] = listCategory;
            ViewData["newProduct"] = listNewProduct;
            ViewData["featureProduct"] = listFeature;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("{slug}")]
        public ActionResult<List<object>> GetCategory(string slug)
        {
            var listProduct = _homeService.GetProduct(slug);
            return listProduct;

        }

        
    }
}
