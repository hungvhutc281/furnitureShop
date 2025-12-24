using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Dto
{
    public class WarehouseDTO
    {
        public int WarehouseId { get; set; } // Mã kho hàng
        public string WarehouseName { get; set; } // Tên kho hàng
        public string Location { get; set; } // Vị trí kho

        public string Phone { get; set; } // Số điện thoại liên hệ
        public int Capacity { get; set; } // Sức chứa kho
    }
}
