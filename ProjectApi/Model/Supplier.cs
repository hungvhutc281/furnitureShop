using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApi.Model
{

    // Bảng Nhà cung cấp
    [Table("Supplier")]
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; } // Mã nhà cung cấp

        [Required, StringLength(1000)]
        public string? SupplierName { get; set; } // Tên nhà cung cấp

        [StringLength(10000)]
        public string? ContactInfo { get; set; } // Thông tin liên hệ
    }


}
