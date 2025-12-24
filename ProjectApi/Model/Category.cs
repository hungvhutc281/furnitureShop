using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApi.Model
{
    // Bảng Danh mục sản phẩm
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; } // Mã danh mục

        [Required, StringLength(1000)]
        public string? CategoryName { get; set; } // Tên danh mục

        [StringLength(100000)]
        public string? Description { get; set; } // Mô tả danh mục

        public ICollection<Product> Products { get; set; }
    }
}
