using FurnitureStore.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Dto;
using ProjectApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
namespace ProjectApi.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly FurnitureStoreContext _context;
        private readonly ILogger<AdminController> _logger; // thêm log xác định lỗi

        public AdminController(FurnitureStoreContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }
        private static List<User> _users = new List<User>();
        // GET: api/admin/user
        [HttpGet("user")]
        public ActionResult<IEnumerable<UserProfileDTO>> GetallUser()
        {
            var user = _context.Users
                .Select(p => new UserProfileDTO
                {
                    UserId = p.UserId,
                    FullName = p.FullName,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    Address = p.Address,
                    DateBirth = p.DateBirth,
                    Image = p.Image,
                    Gender = p.Gender,
                    CreatedAt = p.CreatedAt

                })
                .ToList();

            return Ok(user);
        }

        // GET: api/admin/user/{id}
        [HttpGet("user/{id}")]
        public ActionResult<UserProfileDTO> GetUser(int id)
        {
            var user = _context.Users
                .Where(p => p.UserId == id)
                .Select(p => new UserProfileDTO
                {
                    UserId = p.UserId,
                    FullName = p.FullName,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    Address = p.Address,
                    DateBirth = p.DateBirth,
                    Image = p.Image,
                    Gender = p.Gender,
                    CreatedAt = p.CreatedAt

                })
                .FirstOrDefault();

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/admin/user
        [HttpPost("user")]
        public ActionResult CreateUser([FromBody] UserProfileDTO dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                DateBirth = dto.DateBirth,
                Image = dto.Image,
                Gender = dto.Gender,
                CreatedAt = dto.CreatedAt
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            dto.UserId = user.UserId;
            return CreatedAtAction(nameof(GetallUser), new { id = dto.UserId }, dto);
        }

        private void SendEmail(string email, string name)
        {
            // cài đặt địa chỉ email trong file json
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "hhungzspo2003@gmail.com";
            string smtpPassword = "mszg ptkg qqlp godc";

            try
            {
                var fromAddress = new MailAddress(smtpUsername, "Admin");
                var toAddress = new MailAddress(email, name);
                const string subject = "Thông báo xóa tài khoản";
                string body = $"Xin chào {name},\n\nTài khoản của bạn đã bị xóa do vi phạm chính sách sử dụng. Nếu bạn cho rằng đây là nhầm lẫn, vui lòng liên hệ lại với quản trị viên.\n\nTrân trọng.";

                var smtp = new SmtpClient
                {
                    Host = smtpServer,
                    Port = smtpPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gửi email thất bại: " + ex.Message);
            }
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] AdminLoginDTO loginDto)
        {
            var admin = _context.Admins
                .FirstOrDefault(a => a.userAdmin == loginDto.UserName && a.passwordAdmin == loginDto.Password);

            if (admin == null)
            {
                return Unauthorized(new AdminLoginResponseDTO
                {
                    Success = false,
                    Message = "Tên đăng nhập hoặc mật khẩu không chính xác"
                });
            }

            // đăng nhập thành công
            return Ok(new AdminLoginResponseDTO { Success = true, Message = "Đăng nhập thành công" });
        }


        // DELETE: api/admin/user/{id}
        [HttpDelete("user/{id}")]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(c => c.UserId == id);
                if (user == null)
                    return NotFound();

                string userEmail = user.Email;
                string fullName = user.FullName;

                _context.Users.Remove(user);
                _context.SaveChanges();

                // Gửi email sau khi xóa thành công
                SendEmail(userEmail, fullName);

                return NoContent(); // HTTP 204
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/admin/stats/user-count
        [HttpGet("stats/user-count")]
        public async Task<ActionResult<int>> GetUserCount()
        {
            var userCount = await _context.Users.CountAsync();
            return Ok(userCount);
        }

        // GET: api/admin/stats/daily-sales
        [HttpGet("stats/daily-sales")]
        public async Task<ActionResult<int>> GetDailySales()
        {
            var today = DateTime.Today;
            var dailySales = await _context.OrderDetails
                .Where(od => od.Order.CreatedAt.Date == today && od.Order.Status != "Đã hủy (Chưa thanh toán)") // Assuming only count confirmed orders
                .SumAsync(od => od.Quantity ?? 0); //Tổng số lượng, xử lý các giá trị null tiềm ẩn

            return Ok(dailySales);
        }

        // GET: api/admin/stats/monthly-sales
        [HttpGet("stats/monthly-sales")]
        public async Task<ActionResult<int>> GetMonthlySales()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var monthlySales = await _context.OrderDetails
                .Where(od => od.Order.CreatedAt.Date >= startOfMonth && od.Order.CreatedAt.Date <= today && od.Order.Status != "Đã hủy (Chưa thanh toán)") // Giả sử chỉ tính các đơn hàng đã xác nhận
                .SumAsync(od => od.Quantity ?? 0); // Tổng số lượng, xử lý các giá trị null tiềm ẩn

            return Ok(monthlySales);
        }

        // GET: api/admin/stats/daily-revenue
        [HttpGet("stats/daily-revenue")]
        public async Task<ActionResult<decimal>> GetDailyRevenue()
        {
            var today = DateTime.Today;
            var dailyRevenue = await _context.OrderDetails
                .Where(od => od.Order.CreatedAt.Date == today && od.Order.Status != "Đã hủy (Chưa thanh toán)") // Giả sử chỉ tính doanh thu cho các đơn hàng đã xác nhận
                .SumAsync(od => (od.Quantity ?? 0) * (od.UnitPrice ?? 0)); // tính toán tổng tiền với sản phẩm 

            return Ok(dailyRevenue);
        }

        // GET: api/admin/stats/daily-best-sellers
        [HttpGet("stats/daily-best-sellers")]
        public async Task<ActionResult<IEnumerable<object>>> GetDailyBestSellers([FromQuery] int top = 5) // ;ấy 5 sản phẩm bán chạy nhất
        {
            var today = DateTime.Today;
            var dailyBestSellers = await _context.OrderDetails
                .Where(od => od.Order.CreatedAt.Date == today && od.Order.Status != "Đã hủy (Chưa thanh toán)") //Giả sử chỉ xem xét các đơn hàng đã xác nhận
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantitySold = g.Sum(od => od.Quantity ?? 0)
                })
                .OrderByDescending(x => x.TotalQuantitySold)
                .Take(top)
                .Join(_context.Products, // join với bảng Sản phẩm để lấy tên sản phẩm VÀ giá
                      sale => sale.ProductId,
                      product => product.ProductId,
                      (sale, product) => new
                      {
                          product.ProductId,
                          product.ProductName,
                          product.Image, // Bao gồm hình ảnh nếu cần cho UI
                          product.Price, // thêm giá
                          sale.TotalQuantitySold
                      })
                .ToListAsync();

            return Ok(dailyBestSellers);
        }

        // GET: api/admin/stats/monthly-best-sellers
        [HttpGet("stats/monthly-best-sellers")]
        public async Task<ActionResult<IEnumerable<object>>> GetMonthlyBestSellers([FromQuery] int top = 5) // lấy sản phẩm bán nhiều nhất 
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var monthlyBestSellers = await _context.OrderDetails
                .Where(od => od.Order.CreatedAt.Date >= startOfMonth && od.Order.CreatedAt.Date <= today && od.Order.Status != "Đã hủy (Chưa thanh toán)") //chỉ tính dơn hàng
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantitySold = g.Sum(od => od.Quantity ?? 0)
                })
                .OrderByDescending(x => x.TotalQuantitySold)
                .Take(top)
                 .Join(_context.Products, // join với sản phẩm để lấy giá và sản phẩm
                      sale => sale.ProductId,
                      product => product.ProductId,
                      (sale, product) => new
                      {
                          product.ProductId,
                          product.ProductName,
                          product.Image, //thêm hình ảnh
                          product.Price, 
                          sale.TotalQuantitySold
                      })
                .ToListAsync();

            return Ok(monthlyBestSellers);
        }

        private bool OrderExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderId == id);
        }
    }
}