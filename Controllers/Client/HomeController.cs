using Fastkart.Models;
using Fastkart.Models.Entities;
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

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public IActionResult Index()
        {
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


        public IActionResult NotFound404()
        {
            return View("~/Views/Shared/404.cshtml");
        }

        [HttpGet("{slug}")]
        public ActionResult<List<object>> GetCategory(string slug)
        {
            var listProduct = _homeService.GetProduct(slug);
            return listProduct;

        }

    }
}
