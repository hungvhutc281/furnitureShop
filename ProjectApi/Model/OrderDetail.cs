using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Model
{
    // Bảng Chi tiết đơn hàng
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; } // Mã chi tiết đơn hàng

        [ForeignKey("Order")]
        public int OrderId { get; set; } // Mã đơn hàng
        public Order Order { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; } // Mã sản phẩm
        public Product Product { get; set; }

        public int? Quantity { get; set; } // Số lượng sản phẩm

        public decimal? UnitPrice { get; set; } // Giá đơn vị
    }
}
