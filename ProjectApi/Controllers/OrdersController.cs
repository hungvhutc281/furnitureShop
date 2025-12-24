using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FurnitureStore.Models;
using ProjectApi.Model;
using ProjectApi.Dto;
using Microsoft.Extensions.Configuration;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly FurnitureStoreContext _context;

        public OrdersController(FurnitureStoreContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Orders/checkout
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutOrderDTO dto)
        {
            try
            {
                if (dto == null || dto.Products == null || !dto.Products.Any())
                    return BadRequest("Dữ liệu không hợp lệ");
                if (dto.UserId == null || dto.UserId == 0)
                    return BadRequest("Bạn cần đăng nhập để đặt hàng!");

                var order = new Order
                {
                    UserId = dto.UserId.Value,
                    CustomerName = dto.CustomerName,
                    Age = dto.Age,
                    Phone = dto.Phone,
                    Email = dto.Email,
                    Address = dto.Address,
                    Province = dto.Province,
                    District = dto.District,
                    Ward = dto.Ward,
                    PaymentMethod = dto.PaymentMethod,
                    Status = dto.PaymentMethod == "VNPAY" ? "Chờ xác nhận thanh toán" : "Chờ xác nhận",
                    IsPaid = false,
                    Note = dto.Note
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var p in dto.Products)
                {
                    var detail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = p.ProductId,
                        Quantity = p.Quantity,
                        UnitPrice = p.UnitPrice
                    };
                    _context.OrderDetails.Add(detail);
                }
                await _context.SaveChangesAsync();

                return Ok(new { order.OrderId, order.Status });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        // PUT: api/Orders/{id}/confirm-payment
        [HttpPut("{id}/confirm-payment")]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();
            order.IsPaid = true;
            order.Status = "Đang vận chuyển";
            await _context.SaveChangesAsync();
            return Ok(new { order.OrderId, order.Status, order.IsPaid });
        }

        // PUT: api/Orders/{id}/confirm (Admin xác nhận đơn hàng)
        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();
            order.Status = "Đang vận chuyển";

            // Lấy chi tiết đơn hàng
            var details = await _context.OrderDetails.Where(od => od.OrderId == id).ToListAsync();

            // Trừ số lượng tồn kho cho từng sản phẩm trong đơn hàng
            foreach (var detail in details)
            {
                var product = await _context.Products.FindAsync(detail.ProductId);
                if (product != null && detail.Quantity.HasValue)
                {
                    product.StockQuantity -= detail.Quantity.Value; // trừ đi số đơn hàng mua
                }
            }
            // ---------------------------------------------------------

            await _context.SaveChangesAsync();

            // Lấy chi tiết đơn hàng và sản phẩm (cho email)
            var products = details.Select(od => (
                productName: _context.Products.FirstOrDefault(p => p.ProductId == od.ProductId)?.ProductName ?? "",
                quantity: od.Quantity ?? 0,
                unitPrice: od.UnitPrice ?? 0
            )).ToList();
            decimal totalAmount = details.Sum(od => (od.Quantity ?? 0) * (od.UnitPrice ?? 0));

            // --- thêm tổng phí ship ---
            decimal shippingFee = 30000; // lấy phí ship 30,000 VND
            totalAmount += shippingFee;
            // ----------------------------------------

            // Gửi email xác nhận
            var emailService = new ProjectApi.Services.EmailService(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
            await emailService.SendOrderConfirmedEmail(order.Email, order.OrderId, order.CustomerName, order.Address, order.Phone, order.Status, order.CreatedAt, products, totalAmount);

            return Ok(new { order.OrderId, order.Status });
        }

        // PUT: api/Orders/{id}/reject (Admin từ chối đơn hàng)
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái đơn hàng
            order.Status = "Đã hủy (Chưa thanh toán)"; // Hoặc trạng thái khác phù hợp
            await _context.SaveChangesAsync();

            // Gửi email thông báo cho khách hàng
            var emailService = new ProjectApi.Services.EmailService(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
            await emailService.SendOrderRejectedEmail(order.Email, order.OrderId);

            return Ok(new { order.OrderId, order.Status });
        }


        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        // GET: api/Orders/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetUserOrders(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderDTO
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.CreatedAt,
                    TotalAmount = _context.OrderDetails
                        .Where(od => od.OrderId == o.OrderId)
                        .Sum(od => od.Quantity * od.UnitPrice),
                    OrderStatus = o.Status
                })
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders;
        }

        // GET: api/Orders/{id}/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<object>> GetOrderDetails(int id)
        {
            var order = await _context.Orders
                .Where(o => o.OrderId == id)
                .Select(o => new
                {
                    o.OrderId,
                    o.CustomerName,
                    o.Phone,
                    o.Email,
                    o.Address,
                    o.Province,
                    o.District,
                    o.Ward,
                    o.PaymentMethod,
                    o.Status,
                    OrderDate = o.CreatedAt,
                    o.Note,
                    TotalAmount = _context.OrderDetails
                        .Where(od => od.OrderId == o.OrderId)
                        .Sum(od => od.Quantity * od.UnitPrice),
                    OrderDetails = _context.OrderDetails
                        .Where(od => od.OrderId == o.OrderId)
                        .Select(od => new
                        {
                            od.ProductId,
                            ProductName = _context.Products
                                .Where(p => p.ProductId == od.ProductId)
                                .Select(p => p.ProductName)
                                .FirstOrDefault(),
                            od.Quantity,
                            od.UnitPrice,
                            Total = od.Quantity * od.UnitPrice
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
    }
}
