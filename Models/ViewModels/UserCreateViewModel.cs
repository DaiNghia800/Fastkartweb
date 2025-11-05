using System.ComponentModel.DataAnnotations;

namespace Fastkart.Models.ViewModels
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(100)]
        [RegularExpression(@"^[\p{L}\s']+$", ErrorMessage = "Họ và tên không được chứa số hay kí tự đặc biệt.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        // [EmailAddress] đã xử lý việc kiểm tra định dạng
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ. Phải bắt đầu bằng 0 và có 10 chữ số.")]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100)] // Giữ lại độ dài tối đa
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, bao gồm 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]
        [DataType(DataType.Password)] // Giúp ẩn ký tự khi nhập
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")] // Tên hiển thị
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn quyền cho người dùng")]
        public int RoleUid { get; set; }

        public IFormFile? ImgFile { get; set; }
    }
}
