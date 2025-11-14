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
using System;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace Fastkart.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IUploadService _uploadService;

        public UserService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IUploadService uploadService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _uploadService = uploadService;
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

        public async Task<bool> UpdateUser(Users userModel, List<IFormFile>? imgFiles)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(userModel.Uid);
                if (existingUser == null)
                {
                    return false;
                }

                // XỬ LÝ ẢNH - HỖ TRỢ NHIỀU ẢNH
                var imageUrls = new List<string>();

                // Parse ảnh cũ từ JSON
                if (!string.IsNullOrEmpty(userModel.ImgUser))
                {
                    try
                    {
                        imageUrls = JsonSerializer.Deserialize<List<string>>(userModel.ImgUser) ?? new List<string>();
                    }
                    catch
                    {
                        // Nếu format cũ (string đơn), convert sang array
                        imageUrls.Add(userModel.ImgUser);
                    }
                }

                // Upload ảnh mới
                if (imgFiles != null && imgFiles.Any())
                {
                    foreach (var file in imgFiles)
                    {
                        if (file.Length > 0)
                        {
                            var url = await _uploadService.UploadImageAsync(file);
                            imageUrls.Add(url);
                        }
                    }
                }

                // Lưu dưới dạng JSON array
                existingUser.ImgUser = imageUrls.Count > 0 ? JsonSerializer.Serialize(imageUrls) : "[]";

                existingUser.FullName = userModel.FullName;
                existingUser.Email = userModel.Email;
                existingUser.PhoneNumber = userModel.PhoneNumber;
                existingUser.Address = userModel.Address;
                existingUser.RoleUid = userModel.RoleUid;
                existingUser.UpdatedAt = DateTime.Now;
                existingUser.UpdatedBy = "admin";

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                    if (existingUser.PasswordHash == null ||
                        existingUser.PasswordHash.StartsWith("EXTERNAL_LOGIN_"))
                    {
                        // Cho phép admin thêm password cho user này
                        existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                        existingUser.FullName = model.FullName;
                        existingUser.ImgUser = existingUser.ImgUser;
                        existingUser.PhoneNumber = model.PhoneNumber;
                        existingUser.Address = model.Address;
                        existingUser.RoleUid = model.RoleUid;
                        existingUser.UpdatedAt = DateTime.Now;
                        existingUser.UpdatedBy = "admin";

                        //// XỬ LÝ NHIỀU ẢNH
                        //var imageUrls = new List<string>();

                        //if (!string.IsNullOrEmpty(model.ImgUser))
                        //{
                        //    try
                        //    {
                        //        imageUrls = JsonSerializer.Deserialize<List<string>>(model.ImgUser) ?? new List<string>();
                        //    }
                        //    catch
                        //    {
                        //        imageUrls.Add(model.ImgUser);
                        //    }
                        //}

                        //if (model.ImgFile != null && model.ImgFile.Length > 0)
                        //{
                        //    var url = await _uploadService.UploadImageAsync(model.ImgFile);
                        //    imageUrls.Add(url);
                        //}

                        //existingUser.ImgUser = imageUrls.Count > 0 ? JsonSerializer.Serialize(imageUrls) : "[]";

                        //_context.Users.Update(existingUser);
                        await _context.SaveChangesAsync();

                        return (true, null);
                    }
                    else
                    {
                        return (false, "Email này đã được sử dụng cho tài khoản local.");
                    }
                }

                // 2. Tạo user mới
                var newUser = new Users
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    RoleUid = model.RoleUid,
                    ImgUser = model.ImgUser,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "admin",
                    UpdatedBy = "admin",
                    Deleted = false
                };

                //// XỬ LÝ NHIỀU ẢNH CHO USER MỚI
                //var newImageUrls = new List<string>();

                //if (!string.IsNullOrEmpty(model.ImgUser))
                //{
                //    try
                //    {
                //        newImageUrls = JsonSerializer.Deserialize<List<string>>(model.ImgUser) ?? new List<string>();
                //    }
                //    catch
                //    {
                //        newImageUrls.Add(model.ImgUser);
                //    }
                //}

                //if (model.ImgFile != null && model.ImgFile.Length > 0)
                //{
                //    var url = await _uploadService.UploadImageAsync(model.ImgFile);
                //    newImageUrls.Add(url);
                //}

                //if (newImageUrls.Count == 0)
                //{
                //    newImageUrls.Add("https://res.cloudinary.com/dfeaar87r/image/upload/v1763101391/default-avatar_uek2f1.png");
                //}

                //newUser.ImgUser = JsonSerializer.Serialize(newImageUrls);
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
                    ImgUser = "[\"https://res.cloudinary.com/dfeaar87r/image/upload/v1763101391/default-avatar_uek2f1.png\"]"
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

                        return (existingUser, null);
                    }
                    else
                    {
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
                    ImgUser = "[\"https://res.cloudinary.com/dfeaar87r/image/upload/v1763101391/default-avatar_uek2f1.png\"]"
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
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }
        public async Task<bool> UpdateProfile(ProfileUpdateViewModel model)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(model.Uid);
                if (existingUser == null)
                {
                    return false;
                }

                // Cập nhật Full Name
                if (!string.IsNullOrEmpty(model.FullName))
                {
                    existingUser.FullName = model.FullName.Trim();
                }

                if (!string.IsNullOrEmpty(model.Email))
                {
                    existingUser.Email = model.Email;
                }

                if (!string.IsNullOrEmpty(model.PhoneNumber))
                {
                    existingUser.PhoneNumber = model.PhoneNumber;
                }

                if (!string.IsNullOrEmpty(model.Address))
                {
                    existingUser.Address = model.Address;
                }

                // Xử lý upload ảnh
                if (model.Photo != null && model.Photo.Length > 0)
                {
                    var imageUrls = new List<string>();

                    // Parse ảnh cũ
                    if (!string.IsNullOrEmpty(existingUser.ImgUser))
                    {
                        try
                        {
                            imageUrls = JsonSerializer.Deserialize<List<string>>(existingUser.ImgUser) ?? new List<string>();
                        }
                        catch
                        {
                            imageUrls.Add(existingUser.ImgUser);
                        }
                    }

                    // Thêm ảnh mới
                    var newUrl = await _uploadService.UploadImageAsync(model.Photo);
                    imageUrls.Add(newUrl);

                    existingUser.ImgUser = JsonSerializer.Serialize(imageUrls);
                }

                // Chỉ cập nhật mật khẩu nếu có nhập
                if (!string.IsNullOrEmpty(model.Password))
                {
                    existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                }

                existingUser.UpdatedAt = DateTime.Now;
                existingUser.UpdatedBy = existingUser.FullName;

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<(bool Success, string ErrorMessage)> UpdateUserAsync(UserCreateViewModel model)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(model.Uid);
                if (existingUser == null)
                {
                    return (false, "Không tìm thấy người dùng");
                }

                // Kiểm tra email trùng (ngoại trừ user hiện tại)
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email == model.Email && u.Uid != model.Uid && u.Deleted == false);

                if (emailExists)
                {
                    return (false, "Email này đã được sử dụng bởi người dùng khác");
                }

                // Cập nhật thông tin
                existingUser.FullName = model.FullName;
                existingUser.Email = model.Email;
                existingUser.PhoneNumber = model.PhoneNumber;
                existingUser.Address = model.Address;
                existingUser.RoleUid = model.RoleUid;
                existingUser.ImgUser = model.ImgUser;
                
                // Cập nhật password nếu có nhập mới
                if (!string.IsNullOrEmpty(model.Password))
                {
                    existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                }

                existingUser.UpdatedAt = DateTime.Now;
                existingUser.UpdatedBy = "admin";

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, "Đã xảy ra lỗi hệ thống. Vui lòng thử lại.");
            }
        }

    }
}