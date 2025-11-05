using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Fastkart.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace Fastkart.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<Users> GetAllUsers()
        {
            try
            {
                return _context.Users.Where(t => t.Deleted == false).AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                return new List<Users>();
            }
        }

        public Users GetUserById(int id)
        {
            try
            {
                return _context.Users
                               .AsNoTracking()
                               .FirstOrDefault(u => u.Uid == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateUser(Users userModel, IFormFile? imgFile)
        {
            try
            {
                // 1. Tìm bản ghi gốc trong CSDL
                var existingUser = await _context.Users.FindAsync(userModel.Uid);
                if (existingUser == null)
                {
                    return false; // Không tìm thấy user
                }

                // 2. Xử lý upload file ảnh MỚI (nếu có)
                if (imgFile != null && imgFile.Length > 0)
                {
                    // (Bạn nên xóa file ảnh cũ ở đây nếu có)

                    // Tạo đường dẫn
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "users");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Tạo tên file duy nhất
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imgFile.FileName;
                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    // Lưu file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(fileStream);
                    }

                    // Cập nhật đường dẫn ảnh mới
                    userModel.ImgUser = "/images/users/" + uniqueFileName;
                }

                // 3. Cập nhật các thuộc tính
                existingUser.FullName = userModel.FullName;
                existingUser.Email = userModel.Email;
                existingUser.PhoneNumber = userModel.PhoneNumber;
                existingUser.Address = userModel.Address;
                existingUser.Role = userModel.Role;
                existingUser.ImgUser = userModel.ImgUser; // Cập nhật (dù là ảnh mới hay đường dẫn cũ)

                // 4. Cập nhật thông tin hệ thống
                existingUser.UpdatedAt = DateTime.Now;
                //existingUser.UpdatedBy = Users.Identity.Name ?? "admin"; // Lấy tên user admin đang đăng nhập (hoặc 1 giá trị mặc định)
                existingUser.UpdatedBy = "admin"; // Lấy tên user admin đang đăng nhập (hoặc 1 giá trị mặc định)

                // 5. Lưu thay đổi
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // (Nên log lỗi ex ở đây)
                return false;
            }
        }
        public List<Roles> GetAllRoles()
        {
            try
            {
                return _context.Roles.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Roles>();
            }
        }
        public async Task<bool> DeleteUser(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                user.Deleted = true;
                user.UpdatedAt = DateTime.Now;
                user.UpdatedBy = "admin";
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<(bool Success, string ErrorMessage)> CreateUser(UserCreateViewModel model)
        {
            try
            {
                //Kiểm tra Email đã tồn tại chưa
                if (await _context.Users.AnyAsync(u => u.Email == model.Email && u.Deleted == false))
                {
                    return (false, "Email này đã được sử dụng.");
                }

                var newUser = new Users
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    RoleUid = model.RoleUid,

                    //Hash mật khẩu bằng BCrypt
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),

                    //Thiết lập giá trị mặc định
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "admin",
                    UpdatedBy = "admin",
                    Deleted = false
                };

                //Xử lý file ảnh (nếu có)
                if (model.ImgFile != null && model.ImgFile.Length > 0)
                {
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "users");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImgFile.FileName;
                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImgFile.CopyToAsync(fileStream);
                    }
                    newUser.ImgUser = "/images/users/" + uniqueFileName;
                }
                else
                {
                    newUser.ImgUser = "/images/users/default-avatar.png";
                }

                // Lưu vào CSDL
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return (true, null); // Thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, "Đã xảy ra lỗi hệ thống. Vui lòng thử lại.");
            }
        }
    }
}
