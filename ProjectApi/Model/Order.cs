using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApi.Model
{
    // Bảng Đơn hàng
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; } // Mã đơn hàng
        [ForeignKey("UserId")]
        public int UserId { get; set; } // Id người dùng (nếu có)
        public string CustomerName { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string PaymentMethod { get; set; } // "COD" hoặc "VNPAY"
        public string Status { get; set; } // "Chờ xác nhận", "Chờ thanh toán", "Đang vận chuyển", ...
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsPaid { get; set; } = false; // Đã thanh toán hay chưa
        public string? Note { get; set; } // Ghi chú đơn hàng
    }
}
