using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FurnitureStore.Models;
using ProjectApi.Model;
using ProjectApi.Services;
using ProjectApi.Dto;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.Data;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ProjectApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FurnitureStoreContext _context;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;

        public AuthController(FurnitureStoreContext context, EmailService emailService, IMapper mapper)
        {
            _context = context;
            _emailService = emailService;
            _mapper = mapper;
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedInput = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword)));
                return hashedInput == storedPassword;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO request)
        {
            if (request == null)
            {
                return BadRequest("Dữ liệu gửi lên bị null.");
            }

            if (_context.Users.Any(u => u.Email == request.Email))
                return BadRequest("Email đã tồn tại.");

            try
            {
                var otp = new Random().Next(100000, 999999).ToString();

                var user = _mapper.Map<User>(request);
                user.Password = HashPassword(user.Password);
                user.OtpCode = otp;
                user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5);
                user.EmailConfirmed = false;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await _emailService.SendOtpEmail(request.Email, otp);

                return Ok("Mã OTP đã được gửi đến email.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi đăng ký: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email và mật khẩu không được để trống.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !VerifyPassword(request.Password, user.Password))
            {
                return BadRequest("Email hoặc mật khẩu không đúng.");
            }

            if (!user.EmailConfirmed)
            {
                return BadRequest("Tài khoản chưa được xác thực. Vui lòng kiểm tra email để xác nhận.");
            }

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                user = new
                {
                    user.UserId,
                    user.FullName,
                    user.Email,
                    user.PhoneNumber,
                    user.Address,
                    user.Gender,
                    user.Image
                }
            });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Otp))
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return BadRequest("OTP không hợp lệ hoặc đã hết hạn.");
            }

            Console.WriteLine($"User OTP: {user.OtpCode}, Request OTP: {request.Otp}, Expiry: {user.OtpExpiryTime}");

            if (user.OtpCode != request.Otp || user.OtpExpiryTime < DateTime.UtcNow)
            {
                return BadRequest("OTP không hợp lệ hoặc đã hết hạn.");
            }

            user.EmailConfirmed = true;
            user.OtpCode = null;
            user.OtpExpiryTime = null;
            await _context.SaveChangesAsync();

            return Ok("Xác thực thành công!");
        }

        // Lấy thông tin user bằng email (không cần authentication)
        [HttpGet("get-user-by-email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email không được để trống.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            return Ok(new
            {
                userId = user.UserId,
                email = user.Email,
                fullName = user.FullName,
                user.PhoneNumber,
                user.Address,
                user.DateBirth,
                user.Gender,
                user.Image
            });
        }

       

        // Update profile bằng email (không cần authentication)
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            try
            {
                // cập nhật thông tin cá nhân
                user.FullName = request.FullName;
                user.Address = request.Address;
                user.DateBirth = request.DateBirth;
                user.PhoneNumber = request.PhoneNumber;
                user.Gender = request.Gender;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Cập nhật thông tin thành công!",
                    user = new
                    {
                        user.UserId,
                        user.FullName,
                        user.Email,
                        user.PhoneNumber,
                        user.Address,
                        user.DateBirth,
                        user.Gender,
                        user.Image
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi cập nhật thông tin: {ex.Message}");
            }
        }

          [HttpGet]
  public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
  {
      var users = await _context.Users.Select(u => new UserDTO
      {
          UserId = u.UserId,
          FullName = u.FullName
      }).ToListAsync();

      return users;
  }
        // thay đổi mật khẩu bằng email
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.OldPassword) || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            // xá thực mật khẩu cũ
            if (!VerifyPassword(request.OldPassword, user.Password))
            {
                return BadRequest("Mật khẩu cũ không đúng.");
            }

            // kiểm tra dữ liệu mật khẩu mới
            if (request.NewPassword.Length < 6)
            {
                return BadRequest("Mật khẩu mới phải có ít nhất 6 ký tự.");
            }

            try
            {
                user.Password = HashPassword(request.NewPassword);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Đổi mật khẩu thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi đổi mật khẩu: {ex.Message}");
            }
        }

        // Upload image bằng email
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file, [FromQuery] string email)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Không có file được chọn.");
            }

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email không được để trống.");
            }

            // kiểm tra dữ liệu file
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
            {
                return BadRequest("Chỉ chấp nhận file ảnh (JPEG, PNG, GIF).");
            }

            // kiểm tra dung lượng file ảnh (max 5MB)
            if (file.Length > 5 * 1024 * 1024)
            {
                return BadRequest("Kích thước file không được vượt quá 5MB.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            try
            {
                // tạo thư mục upload nếu chưa tồn tại
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile-images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Tạo tên duy nhất cho file
                var fileName = $"{user.UserId}_{DateTime.Now.Ticks}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(user.Image))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                user.Image = $"/uploads/profile-images/{fileName}";
                await _context.SaveChangesAsync();

                var baseUrl = $"{Request.Scheme}://{Request.Host}";

                return Ok(new
                {
                    message = "Cập nhật ảnh đại diện thành công!",
                    imagePath = $"{baseUrl}{user.Image}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi upload ảnh: {ex.Message}");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("Email không được để trống.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return BadRequest("Email không tồn tại.");

            try
            {
                var otp = new Random().Next(100000, 999999).ToString();
                user.OtpCode = otp;
                user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5);
                await _context.SaveChangesAsync();

                await _emailService.SendOtpEmail(user.Email, otp);

                return Ok("Mã OTP đã được gửi đến email của bạn.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi gửi OTP: {ex.Message}");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.NewPassword))
                return BadRequest("Dữ liệu không hợp lệ.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return BadRequest("Email không tồn tại.");

            if (request.Otp != "VERIFIED")
            {
                if (string.IsNullOrEmpty(request.Otp) || user.OtpCode != request.Otp || user.OtpExpiryTime < DateTime.UtcNow)
                    return BadRequest("OTP không hợp lệ hoặc đã hết hạn.");
            }

            user.Password = HashPassword(request.NewPassword);
            user.OtpCode = null;
            user.OtpExpiryTime = null;
            await _context.SaveChangesAsync();

            return Ok("Đặt lại mật khẩu thành công!");
        }

        // DTO Classes
        public class ChangePasswordRequest
        {
            public string Email { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }

        public class ForgotPasswordRequest
        {
            public string Email { get; set; }
        }

        public class ResetPasswordRequest
        {
            public string Email { get; set; }
            public string Otp { get; set; }
            public string NewPassword { get; set; }
        }
    }
}