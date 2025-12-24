using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Model
{
    
        // Bảng Sản phẩm
        [Table("Product")]
        public class Product
        {
            [Key]
            public int ProductId { get; set; } // Mã sản phẩm

            [Required, StringLength(255)]
            public string ProductName { get; set; } // Tên sản phẩm

            [ForeignKey("Category")]
            public int CategoryId { get; set; } // Mã danh mục
            public Category Category { get; set; } // Liên kết đến bảng Category

            [Required]
            public decimal Price { get; set; } // Giá sản phẩm

            public int StockQuantity { get; set; } // Số lượng tồn kho

            [StringLength(100000)]
            public string? Description { get; set; } // Mô tả sản phẩm

            [StringLength(500)]
            public string? Material { get; set; } // Chất liệu

            [StringLength(100)]
            public string? Dimensions { get; set; } // Kích thước

            [StringLength(255)]
            public string? Image { get; set; } // Ảnh đại diện chính

            public DateTime CreatedAt { get; set; } = DateTime.Now; // Ngày tạo sản phẩm
            public DateTime UpdatedAt { get; set; } = DateTime.Now; // Ngày cập nhật sản phẩm
        }

    }



