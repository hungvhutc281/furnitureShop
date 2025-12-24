using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Model
{
    // Bảng Hình ảnh sản phẩm
    [Table("ProductImage")]
    public class ProductImage
    {
        [Key]
        public int ImageId { get; set; } // Mã hình ảnh

        [ForeignKey("Product")]
        public int ProductId { get; set; } // Mã sản phẩm
        public Product Product { get; set; } // Liên kết đến bảng Product

        [StringLength(255)]
        public string? ImageUrl { get; set; } // Đường dẫn hình ảnh
    }

}
