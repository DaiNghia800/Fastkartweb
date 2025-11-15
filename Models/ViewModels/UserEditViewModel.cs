using System.ComponentModel.DataAnnotations;

namespace Fastkart.Models.ViewModels
{
    public class UserEditViewModel
    {
        public int Uid { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        public string? Address { get; set; }

        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng 0 và có 10 chữ số")]
        public string? PhoneNumber { get; set; }

        // Password không bắt buộc khi edit
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string? ConfirmPassword { get; set; }

        //public IFormFile? ImgFile { get; set; }

        public string? ImgUser { get; set; } // Đường dẫn ảnh hiện tại

        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        public int RoleUid { get; set; }
    }
}