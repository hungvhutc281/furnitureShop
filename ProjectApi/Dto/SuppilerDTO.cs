using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Dto
{
    public class SuppilerDTO
    {
        public int SupplierId { get; set; } // Mã nhà cung cấp

        public string? SupplierName { get; set; } // Tên nhà cung cấp

        public string? ContactInfo { get; set; } // Thông tin liên hệ
    }
}
