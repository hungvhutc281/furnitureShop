using ProjectApi.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApi.Dto
{
    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; } // Mã chi tiết đơn hàng

        public int OrderId { get; set; } // Mã đơn hàng

        public int ProductId { get; set; } // Mã sản phẩm

        public int? Quantity { get; set; } // Số lượng sản phẩm

        public decimal? UnitPrice { get; set; } // Giá đơn vị
    }
}
