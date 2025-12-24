using System;

namespace ProjectApi.Dto
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; } // Mã thanh toán

        public int OrderId { get; set; } // Mã đơn hàng

        public decimal AmountPaid { get; set; } // Số tiền đã thanh toán

        public DateTime? PaymentDate { get; set; } = DateTime.Now; // Ngày thanh toán
    }
}
