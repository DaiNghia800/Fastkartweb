using Fastkart.Models.Entities;
using Fastkart.Models.ViewModels;
using Fastkart.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Fastkart.Controllers.Admin
{
    [Authorize]
    [Route("/admin/user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 5;
            var paginatedUsers = await _userService.GetAllUsersAsync(pageNumber, pageSize);
            return View("~/Views/Admin/User/index.cshtml", paginatedUsers);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var allRoles = _userService.GetAllRoles();
            ViewData["RolesList"] = new SelectList(allRoles, "Uid", "RoleName");
            var model = new UserCreateViewModel();
            return View("~/Views/Admin/User/create.cshtml", model);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] UserCreateViewModel model)
        {
            //Tải lại RolesList phòng trường hợp phải trả về View
            var allRoles = _userService.GetAllRoles();
            ViewData["RolesList"] = new SelectList(allRoles, "Uid", "RoleName", model.RoleUid);

            //Kiểm tra các ràng buộc (Regex, Required...) trong ViewModel
            if (!ModelState.IsValid)
            {
                // Nếu không hợp lệ, trả về View để hiển thị lỗi
                return View("~/Views/Admin/User/Create.cshtml", model);
            }

            // Nếu hợp lệ, gọi Service để tạo user
            var (success, errorMessage) = await _userService.CreateUser(model);

            if (success)
            {
                //Nếu thành công, quay về trang danh sách
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View("~/Views/Admin/User/Create.cshtml", model);
            }
        }

        [HttpGet("get-user-detail")]
        public IActionResult GetUserDetail(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }
            return PartialView("~/Views/Admin/User/userdetail.cshtml", user);
        }

        [HttpGet("get-user-edit")]
        public IActionResult GetUserEdit(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }
            var allRoles = _userService.GetAllRoles();
            ViewData["RolesList"] = new SelectList(allRoles, "Uid", "RoleName", user.RoleUid);
            return PartialView("~/Views/Admin/User/useredit.cshtml", user);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateInfoUser([FromForm] Users userModel, IFormFile? imgFile)
        {
            bool result = await _userService.UpdateUser(userModel, imgFile);

            if (result)
            {

                return Json(new { success = true, message = "Cập nhật người dùng thành công!" });
            }
            else
            {
                return Json(new { success = false, message = "Cập nhật thất bại. Vui lòng nhập đầy đủ thông tin." });
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            if (id <= 0)
            {
                return Json(new { success = false, message = "ID không hợp lệ." });
            }
            //Lấy ID của user hiện tại đang đăng nhập
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserIdClaim) || !int.TryParse(currentUserIdClaim, out int currentUserId))
            {
                return Json(new { success = false, message = "Không thể xác định người dùng hiện tại." });
            }

            if( id == currentUserId)
            {
                return Json(new { success = false, message = "Bạn không thể xóa chính tài khoản của mình!" });
            }   
            var result = await _userService.DeleteUser(id);

            if (result)
            {
                return Json(new { success = true, message = "Đã xóa thành công." });
            }
            else
            {
                return Json(new { success = false, message = "Xóa thất bại vui lòng thử lại." });
            }
        }
    }
}
