using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace ProjectFinal.ProjectApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly FurnitureStoreContext _context;

        public AuthController(FurnitureStoreContext context)
        {
            _context = context;
        }

        //[HttpGet("get-user-by-email")]
        //public async Task<IActionResult> GetUserByEmail(string email)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        //    if (user == null) return NotFound();
        //    return Ok(new { userId = user.UserId, email = user.Email });
        //}

        // New API: Upload Avatar
        [HttpPost("upload-avatar")]
        [Authorize]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Vui lòng chọn ảnh.");

                // xác thực người dùng qua userID
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized("Không thể xác định người dùng.");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null) return NotFound("Người dùng không tồn tại.");

                var folderPath = Path.Combine("wwwroot", "avatars");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                user.Image = $"/avatars/{fileName}";
                await _context.SaveChangesAsync();

                return Ok(new { image = user.Image });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi tải lên ảnh: {ex.Message}");
            }
        }
    }
} 