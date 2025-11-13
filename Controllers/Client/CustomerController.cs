using Fastkart.Models.ViewModels;
using Fastkart.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fastkart.Controllers.Client
{
    [Authorize]
    [Route("/customer")]
    public class CustomerController : Controller
    {
        private readonly IUserService _userService;

        public CustomerController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("get-my-profile")]
        public IActionResult GetMyProfile()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _userService.GetUserById(currentUserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return PartialView("~/Views/Customer/MyProfile.cshtml", user);
        }

        // GET: Settings Modal - Trả về ProfileUpdateViewModel
        [HttpGet("get-settings")]
        public IActionResult GetSettings()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _userService.GetUserById(currentUserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var model = new ProfileUpdateViewModel
            {
                Uid = user.Uid,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return PartialView("~/Views/Customer/ProfileSetting.cshtml", model);
        }

        // POST: Update Profile
        [HttpPost("update-profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileUpdateViewModel model)
        {
            // Validate password if provided
            if (!string.IsNullOrEmpty(model.Password))
            {
                if (string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    return Json(new { success = false, message = "Please confirm your password." });
                }

                if (model.Password != model.ConfirmPassword)
                {
                    return Json(new { success = false, message = "Passwords do not match." });
                }
            }

            // Remove password validation từ ModelState nếu không nhập
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.Remove(nameof(model.Password));
                ModelState.Remove(nameof(model.ConfirmPassword));
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
                return Json(new { success = true, message = "Profile updated successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "Update failed. Please try again." });
            }
        }
    }
}