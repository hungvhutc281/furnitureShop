using ProjectApi.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Dto
{
    public class OrderDTO
    {
        public int OrderId { get; set; } // Mã đơn hàng

        public int UserId { get; set; } 

        public DateTime? OrderDate { get; set; } = DateTime.Now; // Ngày đặt hàng

        public decimal? TotalAmount { get; set; } // Tổng tiền đơn hàng

        public string? OrderStatus { get; set; } // Trạng thái đơn hàng
    }

    // DTO cho đặt hàng (checkout)
    public class CheckoutOrderDTO
    {
        public int? UserId { get; set; } // Id người dùng (nếu có)
        public string CustomerName { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        // tỉnh
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string PaymentMethod { get; set; } // "COD" hoặc "VNPAY"
        public string? Note { get; set; }
        public List<OrderProductDTO> Products { get; set; }
    }

    public class OrderProductDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
