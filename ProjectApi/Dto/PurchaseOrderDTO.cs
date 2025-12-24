using ProjectApi.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApi.Dto
{
    public class PurchaseOrderDTO
    {
        public int PurchaseOrderId { get; set; } // Mã đơn nhập hàng

        public int SupplierId { get; set; } // Mã nhà cung cấp

        public DateTime PurchaseDate { get; set; } = DateTime.Now; // Ngày nhập hàng

        public decimal? TotalCost { get; set; } // Tổng chi phí nhập hàng
    }
}
