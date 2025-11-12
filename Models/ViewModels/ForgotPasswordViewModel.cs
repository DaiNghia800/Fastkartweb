using System.ComponentModel.DataAnnotations;

namespace Fastkart.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
    }

    public class VerifyOtpViewModel
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mã OTP là bắt buộc")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP phải có 6 ký tự")]
        public string OtpCode { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mã OTP là bắt buộc")]
        public string OtpCode { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }
    }
}