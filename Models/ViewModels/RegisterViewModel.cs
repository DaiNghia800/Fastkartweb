using System.ComponentModel.DataAnnotations;

namespace Fastkart.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [RegularExpression(@"^[\p{L}\s']+$", ErrorMessage = "Họ và tên không được chứa số hay kí tự đặc biệt.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý với điều khoản và chính sách bảo mật")]
        public bool AgreeToTerms { get; set; }
    }
}