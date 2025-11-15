using Fastkart.Models.ViewModels;
using Fastkart.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fastkart.Controllers.Admin
{
    [Authorize(Policy = "NoCustomer")]
    [Route("admin/settings")]
    public class SettingController : Controller
    {
        private readonly IUserService _userService;

        public SettingController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile/{id}")]
        public IActionResult Index(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }

            var model = new UserCreateViewModel
            {
                Uid = user.Uid,
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                ImgUser = user.ImgUser,
                RoleUid = user.RoleUid
            };

            var allRoles = _userService.GetAllRoles();
            ViewData["RolesList"] = new SelectList(allRoles, "Uid", "RoleName", user.RoleUid);

            return View("~/Views/Admin/Setting/Index.cshtml", model);
        }

        [HttpPost("profile/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(int id, [FromForm] UserCreateViewModel model)
        {
            if (id != model.Uid)
            {
                return BadRequest("ID không khớp");
            }

            // Nếu không nhập password mới, bỏ qua validation password
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.Remove(nameof(model.Password));
                ModelState.Remove(nameof(model.ConfirmPassword));
            }

            // Tải lại RolesList phòng trường hợp phải trả về View
            var allRoles = _userService.GetAllRoles();
            ViewData["RolesList"] = new SelectList(allRoles, "Uid", "RoleName", model.RoleUid);

            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/Setting/Index.cshtml", model);
            }

            // Gọi Service để update
            var (success, errorMessage) = await _userService.UpdateUserAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToAction("Index", new { id = model.Uid });
            }
            else
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View("~/Views/Admin/Setting/Index.cshtml", model);
            }
        }
    }
}