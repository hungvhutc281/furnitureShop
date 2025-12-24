using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Model
{
    // Bảng Nhập hàng
    [Table("PurchaseOrder")]
    public class PurchaseOrder
    {
        [Key]
        public int PurchaseOrderId { get; set; } // Mã đơn nhập hàng

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; } // Mã nhà cung cấp
        public Supplier Supplier { get; set; } // Liên kết đến bảng Supplier

        public DateTime PurchaseDate { get; set; } = DateTime.Now; // Ngày nhập hàng

        public decimal? TotalCost { get; set; } // Tổng chi phí nhập hàng
    }
}
