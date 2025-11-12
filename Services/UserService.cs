using Fastkart.Helpers;
using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Fastkart.Models.ViewModels;
using Fastkart.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Fastkart.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public UserService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<PaginatedList<Users>> GetAllUsersAsync(int pageIndex, int pageSize)
        {
            // 1. Tạo IQueryable (chưa gọi CSDL)
            var query = _context.Users
                                .Where(u => u.Deleted == false)
                                .Include(u => u.Role) // (Nên Include Role ở đây)
                                .AsNoTracking()
                                .OrderBy(u => u.FullName); // Bắt buộc phải OrderBy

            // 2. Gọi hàm CreateAsync để thực thi truy vấn
            return await PaginatedList<Users>.CreateAsync(query, pageIndex, pageSize);
        }

        public Users GetUserById(int id)
        {
            try
            {
                return _context.Users
                               .Include(u => u.Role)
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
                // 1. Tìm user với email này
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Deleted == false);

                if (existingUser != null)
                {
                    // ✅ Kiểm tra xem user này có phải từ external login không
                    if (existingUser.PasswordHash == null ||
                        existingUser.PasswordHash.StartsWith("EXTERNAL_LOGIN_"))
                    {
                        // Cho phép admin thêm password cho user này
                        existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                        existingUser.FullName = model.FullName; // Update thông tin
                        existingUser.PhoneNumber = model.PhoneNumber;
                        existingUser.Address = model.Address;
                        existingUser.RoleUid = model.RoleUid;
                        existingUser.UpdatedAt = DateTime.Now;
                        existingUser.UpdatedBy = "admin";

                        // Xử lý ảnh (nếu có)
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
                            existingUser.ImgUser = "/images/users/" + uniqueFileName;
                        }

                        _context.Users.Update(existingUser);
                        await _context.SaveChangesAsync();

                        return (true, null); // Thành công - Đã link password
                    }
                    else
                    {
                        // User đã có password (tài khoản local) - Không cho phép trùng
                        return (false, "Email này đã được sử dụng cho tài khoản local.");
                    }
                }

                // 2. Nếu không có user nào → Tạo mới như bình thường
                var newUser = new Users
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    RoleUid = model.RoleUid,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "admin",
                    UpdatedBy = "admin",
                    Deleted = false
                };

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

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, "Đã xảy ra lỗi hệ thống. Vui lòng thử lại.");
            }
        }
        public Users Login(string username, string password)
        {
            try
            {

                var user = _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Email == username.ToLower().Trim() && u.Deleted == false);

                //kiểm tra user tồn tại và verify password bằng BCrypt
                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<Users> FindOrCreateExternalUserAsync(
            string email,
            string fullName,
            string providerUserId,
            string provider)
        {
            try
            {
                // 1. Tìm user bằng email
                var user = await _context.Users
                                     .Include(u => u.Role)
                                     .FirstOrDefaultAsync(u => u.Email == email && u.Deleted == false);

                if (user != null)
                {
                    return user;
                }

                // 2. Tạo user mới
                var customerRole = await _context.Roles
                    .FirstOrDefaultAsync(r => r.RoleName == WebConstants.ROLE_CUSTOMER);

                if (customerRole == null)
                {
                    throw new Exception("Không tìm thấy Role 'Customer'.");
                }


                string createdByValue = provider switch
                {
                    "Google" => "GoogleAuth",
                    "Facebook" => "FacebookAuth",
                    _ => "ExternalAuth"
                };

                var newUser = new Users
                {
                    FullName = fullName ?? email,
                    Email = email,
                    RoleUid = customerRole.Uid,
                    PasswordHash = $"EXTERNAL_LOGIN_{provider.ToUpper()}_{Guid.NewGuid()}", // Thêm provider vào hash
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = createdByValue,
                    UpdatedBy = createdByValue,
                    Deleted = false,
                    ImgUser = "/images/users/default-avatar.png"
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                newUser.Role = customerRole;
                return newUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FindOrCreateExternalUserAsync: {ex.Message}");
                throw;
            }
        }
        public async Task<(Users User, string ErrorMessage)> RegisterUserAsync(RegisterViewModel model)
        {
            try
            {
                // 1. Tìm user với email này
                var existingUser = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Deleted == false);

                if (existingUser != null)
                {
                    //  Kiểm tra xem user này có phải từ external login không
                    if (existingUser.PasswordHash == null ||
                        existingUser.PasswordHash.StartsWith("EXTERNAL_LOGIN_"))
                    {
                        //  Cho phép user tự link password
                        existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                        existingUser.FullName = model.FullName;
                        existingUser.UpdatedAt = DateTime.Now;
                        existingUser.UpdatedBy = "SelfRegister";

                        _context.Users.Update(existingUser);
                        await _context.SaveChangesAsync();

                        return (existingUser, null); // Thành công - Đã link password
                    }
                    else
                    {
                        // User đã có password - Không cho đăng ký lại
                        return (null, "Email này đã được sử dụng.");
                    }
                }

                // 2. Tạo user mới
                var customerRole = await _context.Roles
                    .FirstOrDefaultAsync(r => r.RoleName == WebConstants.ROLE_CUSTOMER);

                if (customerRole == null)
                {
                    return (null, "Lỗi hệ thống: Không tìm thấy vai trò 'Customer'.");
                }

                var newUser = new Users
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    RoleUid = customerRole.Uid,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "SelfRegister",
                    UpdatedBy = "SelfRegister",
                    Deleted = false,
                    ImgUser = "/images/users/default-avatar.png"
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                newUser.Role = customerRole;
                return (newUser, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (null, "Đã xảy ra lỗi hệ thống. Vui lòng thử lại.");
            }
        }

        public async Task<(bool Success, string Message)> GenerateOtpAsync(string email)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email.ToLower() && u.Deleted == false);

                if (user == null)
                {
                    return (false, "Email không tồn tại trong hệ thống");
                }

                // Tạo OTP ngẫu nhiên 6 chữ số
                var random = new Random();
                var otpCode = random.Next(100000, 999999).ToString();

                // Lưu OTP và thời gian hết hạn (5 phút)
                user.OtpCode = otpCode;
                user.OtpExpiry = DateTime.Now.AddMinutes(5);
                user.UpdatedAt = DateTime.Now;
                user.UpdatedBy = "System";

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                // Gửi email
                await SendOtpEmailAsync(email, otpCode, user.FullName);

                return (true, "Mã OTP đã được gửi đến email của bạn");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GenerateOtpAsync: {ex.Message}");
                return (false, "Có lỗi xảy ra khi gửi mã OTP");
            }
        }

        public async Task<(bool Success, string Message)> VerifyOtpAsync(string email, string otpCode)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email.ToLower() && u.Deleted == false);

                if (user == null)
                {
                    return (false, "Email không tồn tại");
                }

                if (string.IsNullOrEmpty(user.OtpCode))
                {
                    return (false, "Chưa có mã OTP nào được tạo");
                }

                if (user.OtpExpiry < DateTime.Now)
                {
                    return (false, "Mã OTP đã hết hạn");
                }

                if (user.OtpCode != otpCode)
                {
                    return (false, "Mã OTP không chính xác");
                }

                return (true, "Xác thực OTP thành công");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in VerifyOtpAsync: {ex.Message}");
                return (false, "Có lỗi xảy ra khi xác thực OTP");
            }
        }

        public async Task<(bool Success, string Message)> ResetPasswordAsync(string email, string otpCode, string newPassword)
        {
            try
            {
                // Xác thực OTP trước
                var (isValid, message) = await VerifyOtpAsync(email, otpCode);
                if (!isValid)
                {
                    return (false, message);
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email.ToLower() && u.Deleted == false);

                if (user == null)
                {
                    return (false, "Email không tồn tại");
                }

                // Cập nhật mật khẩu mới
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.OtpCode = null; // Xóa OTP sau khi sử dụng
                user.OtpExpiry = null;
                user.UpdatedAt = DateTime.Now;
                user.UpdatedBy = "System";

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return (true, "Đặt lại mật khẩu thành công");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ResetPasswordAsync: {ex.Message}");
                return (false, "Có lỗi xảy ra khi đặt lại mật khẩu");
            }
        }

        private async Task SendOtpEmailAsync(string toEmail, string otpCode, string userName)
        {
            try
            {
                Console.WriteLine("=== BẮT ĐẦU GỬI EMAIL ===");

                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderPassword = _configuration["EmailSettings:SenderPassword"];

                using var smtpClient = new SmtpClient(smtpServer, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    Timeout = 30000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, "Fastkart"),
                    Subject = "Mã OTP đặt lại mật khẩu",
                    Body = $@"
                <html>
                <body>
                    <h2>Xin chào {userName},</h2>
                    <p>Bạn đã yêu cầu đặt lại mật khẩu. Mã OTP của bạn là:</p>
                    <h1 style='color: #4CAF50;'>{otpCode}</h1>
                    <p>Mã OTP này có hiệu lực trong 5 phút.</p>
                    <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
                </body>
                </html>
            ",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);

            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine(" SMTP ERROR!");
                Console.WriteLine($"Status Code: {smtpEx.StatusCode}");
                Console.WriteLine($"Message: {smtpEx.Message}");
                Console.WriteLine($"Inner Exception: {smtpEx.InnerException?.Message}");
                Console.WriteLine($"Stack Trace: {smtpEx.StackTrace}");
                throw new Exception($"Lỗi gửi email: {smtpEx.Message}", smtpEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" GENERAL ERROR!");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public List<string> getPermissionRole(int roleId)
        {
            try
            {
                return _context.Permissions
                    .Include(p => p.PermissionType)
                    .Include(p => p.Function)
                    .Where(p => p.RoleId == roleId)
                    .Select(p => p.Function.Code + "_" + p.PermissionType.Code)
                    .ToList();
            } catch(Exception ex)
            {
                return new List<string>();
            }
        }
    }
}
