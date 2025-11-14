using Microsoft.AspNetCore.Mvc;
using Fastkart.Services;

namespace Fastkart.Controllers.Client
{
    //[Route("/cart")]
    //public class CartController : Controller
    //{
    //    [HttpGet("")]
    //    public IActionResult Index()
    //    {
    //        return View("~/Views/Cart/Index.cshtml");
    //    }
    //}

    [Route("/cart")]
    public class CartController : Controller
    {
        // 1. Khai báo service
        private readonly CartService _cartService;

        // 2. Tiêm service vào constructor
        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        // 3. Cập nhật hàm Index
        [HttpGet] // Chỉ định rõ là [HttpGet] cho trang chính
        public IActionResult Index()
        {
            // Lấy dữ liệu giỏ hàng từ service
            var cartItems = _cartService.GetCartItems();

            // Lấy tổng tiền (chưa gồm ship)
            ViewBag.Subtotal = _cartService.GetSubtotal();

            // Gửi danh sách sản phẩm ra View
            return View("~/Views/Cart/Index.cshtml", cartItems);
        }

        // 4. Thêm hàm "Thêm vào giỏ"
        // (Bạn sẽ gọi action này từ trang sản phẩm)
        [HttpPost("add")]
        public IActionResult Add(int productId, int quantity = 1)
        {
            _cartService.AddToCart(productId, quantity);

            // Sau khi thêm, chuyển hướng về trang giỏ hàng
            return RedirectToAction("Index");
        }

        // 5. Thêm hàm "Xóa khỏi giỏ"
        // (Bạn sẽ gọi action này từ nút "Remove" trong giỏ hàng)
        [HttpGet("remove")]
        public IActionResult Remove(int productId)
        {
            _cartService.RemoveFromCart(productId);

            // Tải lại trang giỏ hàng
            return RedirectToAction("Index");
        }
    }
}
