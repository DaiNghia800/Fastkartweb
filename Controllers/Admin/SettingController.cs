using Fastkart.Models.ViewModels;
using Fastkart.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fastkart.Controllers.Admin
{
    [Route("/admin/settings/profile")]
    public class SettingController : Controller
    {
        private readonly IUserService _userService;
        public SettingController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _userService.GetUserById(currentUserId);

            if (user == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var model = new ProfileUpdateViewModel
            {
                Uid = user.Uid,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return View("~/Views/Admin/Setting/Index.cshtml", model);
        }
        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileUpdateViewModel model)
        {
            // Kiểm tra nếu có nhập password
            if (!string.IsNullOrEmpty(model.Password))
            {
                // Bắt buộc phải có confirm password
                if (string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    return Json(new { success = false, message = "Vui lòng xác nhận mật khẩu." });
                }

                // Kiểm tra khớp
                if (model.Password != model.ConfirmPassword)
                {
                    return Json(new { success = false, message = "Mật khẩu xác nhận không khớp." });
                }
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            var result = await _userService.UpdateProfile(model);

            if (result)
            {
                return Json(new { success = true, message = "Cập nhật thông tin thành công!" });
            }
            else
            {
                return Json(new { success = false, message = "Cập nhật thất bại. Vui lòng thử lại." });
            }
        }
    }
}