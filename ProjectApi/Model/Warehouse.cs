using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApi.Model
{// Bảng Kho hàng
    [Table("Warehouse")]
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; } // Mã kho hàng

        public string Location { get; set; } // Vị trí kho

        [Required, StringLength(2000)]
        public string? WarehouseName { get; set; } // Tên kho hàng
        public string? Phone { get; set; } // Số điện thoại liên hệ
        public int Capacity { get; set; } // Sức chứa kho
    }
}
