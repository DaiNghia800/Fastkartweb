using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Fastkart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks; // <-- Đảm bảo bạn có thư viện này
using System.Collections.Generic; // <-- Thêm thư viện này

namespace Fastkart.Controllers.Client
{
    [Route("checkout")]
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly CartService _cartService;
        private readonly ApplicationDbContext _context;

        public CheckoutController(CartService cartService, ApplicationDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. Lấy User ID thật
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdStr, out int userId);

            // 2. Lấy giỏ hàng từ Session
            var cartItems = _cartService.GetCartItems();
            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            // --- SỬA LỖI Ở ĐÂY ---

            // 3. Lấy 1 User từ CSDL
            var currentUser = await _context.Users.FindAsync(userId);

            // 4. Tạo một danh sách địa chỉ "giả" (List) chỉ chứa 1 địa chỉ thật
            var userAddresses = new List<string>();
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.Address))
            {
                userAddresses.Add(currentUser.Address); // Thêm địa chỉ duy nhất vào List
            }

            // 5. Tạo một ViewModel mới để gửi cả 2 sang View
            // (Bạn sẽ cần tạo file ViewModel này)
            var viewModel = new CheckoutPageViewModel
            {
                CartItems = cartItems,
                // Gán danh sách chỉ có 1 địa chỉ
                AddressesAsStrings = userAddresses
            };

            return View("~/Views/Order/Checkout.cshtml", viewModel);
        }
    }
}