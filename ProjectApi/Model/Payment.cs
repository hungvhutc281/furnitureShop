using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace ProjectApi.Model
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; } // Mã thanh toán

        [ForeignKey("Order")]
        public int OrderId { get; set; } // Mã đơn hàng
        public Order Order { get; set; } // Liên kết đến bảng Order

        public decimal AmountPaid { get; set; } // Số tiền đã thanh toán

        public DateTime? PaymentDate { get; set; } = DateTime.Now; // Ngày thanh toán
    }
}
