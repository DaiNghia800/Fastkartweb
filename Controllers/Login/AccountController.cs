using Fastkart.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Fastkart.Models.ViewModels;
using Fastkart.Services.IServices;

namespace Fastkart.Controllers.Login
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("/login")]
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == WebConstants.ROLE_ADMIN)
                {
                    return Redirect("/admin/dashboard");
                }
                else if (userRole == WebConstants.ROLE_CUSTOMER)
                {
                    return Redirect("/home");
                }
            }

            return View("~/Views/Account/index.cshtml");
        }

        [Route("/sign-up")]
        public IActionResult SignUp()
        {
            return View("~/Views/Account/sign-up.cshtml", new RegisterViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> LoginToSystem(string username, string password, bool rememberMe)
        {
            try
            {
                var accounts = _userService.Login(username, password);
                if (accounts != null)
                {
                    var permissions = _userService.getPermissionRole(accounts.RoleUid);

                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, accounts.Uid.ToString()));
                    claims.Add(new Claim(ClaimTypes.Email, accounts.Email));
                    claims.Add(new Claim(ClaimTypes.Name, accounts.FullName));
                    claims.Add(new Claim(ClaimTypes.Role, accounts.Role.RoleName));

                   
                    foreach (var p in permissions)
                    {
                        claims.Add(new Claim("permission", p));
                    }

                    var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(30) : null
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), authProperties);
                    string redirectUrl = (accounts.Role.RoleName == WebConstants.ROLE_CUSTOMER) ? "/home" : "/admin/dashboard";
                    return Json(new { status = WebConstants.SUCCESS, success = true, message = "Đăng nhập thành công", redirectUrl = redirectUrl });
                }
                else
                {
                    return Json(new { status = WebConstants.ERROR, success = false, message = "Tài khoản hoặc mật khẩu không chính xác" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = WebConstants.ERROR, success = false, message = "Lỗi đăng nhập", error = ex.ToString() });
            }
        }

        [HttpPost]
        [Route("/Account/SignUpUser")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SignUpUser(RegisterViewModel model)
        {
            // 1. Kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new
                {
                    status = WebConstants.ERROR,
                    success = false,
                    message = "Dữ liệu không hợp lệ",
                    errors = errors
                });
            }

            // 2. Gọi Service để tạo user
            var (newUser, errorMessage) = await _userService.RegisterUserAsync(model);

            if (newUser == null)
            {
                return Json(new
                {
                    status = WebConstants.ERROR,
                    success = false,
                    message = errorMessage
                });
            }

            // 3. Thành công
            return Json(new
            {
                status = WebConstants.SUCCESS,
                success = true,
                message = "Đăng ký tài khoản thành công! Vui lòng đăng nhập."
            });
        }

        [HttpPost("/Account/ExternalLogin")]
        public IActionResult ExternalLogin(string provider)
        {
            // URL mà Google sẽ gọi lại sau khi xác thực thành công
            var callbackUrl = Url.Action("ExternalLoginCallback", "Account", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = callbackUrl };
            // Chuyển hướng người dùng đến Google
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback()
        {
            try
            {
                // 1. Xác thực với External Provider
                var authenticateResult = await HttpContext.AuthenticateAsync("External");

                if (!authenticateResult.Succeeded)
                {
                    TempData["ErrorMessage"] = "Xác thực thất bại. Vui lòng thử lại.";
                    return RedirectToAction("Index");
                }

                // 2. Lấy thông tin từ External Provider
                var externalClaims = authenticateResult.Principal.Claims.ToList();

                var email = externalClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var fullName = externalClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var providerUserId = externalClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                // Lấy provider name
                var providerName = authenticateResult.Properties?.Items[".AuthScheme"]
                                 ?? authenticateResult.Principal.Identity?.AuthenticationType
                                 ?? "Unknown";

                // Normalize provider name
                if (providerName.Contains("Google", StringComparison.OrdinalIgnoreCase))
                {
                    providerName = "Google";
                }
                else if (providerName.Contains("Facebook", StringComparison.OrdinalIgnoreCase))
                {
                    providerName = "Facebook";
                }

                if (string.IsNullOrEmpty(email))
                {
                    TempData["ErrorMessage"] = "Không lấy được email. Vui lòng thử lại.";
                    return RedirectToAction("Index");
                }

                // ĐÚNG: Truyền cả 4 tham số
                var user = await _userService.FindOrCreateExternalUserAsync(
                    email,
                    fullName,
                    providerUserId,
                    providerName
                );

                if (user == null || user.Role == null)
                {
                    TempData["ErrorMessage"] = "Không thể tạo tài khoản. Vui lòng liên hệ quản trị viên.";
                    return RedirectToAction("Index");
                }

                // 4. Sign out External scheme
                await HttpContext.SignOutAsync("External");

                // 5. Tạo Claims cho user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Uid.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FullName ?? email),
                    new Claim(ClaimTypes.Role, user.Role.RoleName)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(30),
                    AllowRefresh = true
                };

                // 6. Sign in với Cookie Authentication
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // 7. Redirect dựa trên Role
                string redirectUrl = user.Role.RoleName == WebConstants.ROLE_ADMIN
                    ? "/admin/dashboard"
                    : "/home";

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in ExternalLoginCallback: {ex.Message}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi đăng nhập. Vui lòng thử lại.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SendOtp(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "Email không được để trống" });
            }

            var (success, message) = await _userService.GenerateOtpAsync(email);
            return Json(new { success, message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> VerifyOtp(string email, string otpCode)
        {
            var (success, message) = await _userService.VerifyOtpAsync(email, otpCode);
            return Json(new { success, message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ResetPassword(string email, string otpCode, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                return Json(new { success = false, message = "Mật khẩu xác nhận không khớp" });
            }

            var (success, message) = await _userService.ResetPasswordAsync(email, otpCode, newPassword);
            return Json(new { success, message });
        }

        [Route("/forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View("~/Views/Account/forgotpassword.cshtml");
        }
        [Route("/otp")]
        public IActionResult OTP()
        {
            return View("~/Views/Account/otp.cshtml");
        }

        [Route("/reset-password")]
        public IActionResult ResetPasswordPage()
        {
            return View("~/Views/Account/resetpassword.cshtml");
        }

        [HttpGet]
        [Route("/Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Account");
        }
    }
}
