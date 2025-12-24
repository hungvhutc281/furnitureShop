using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApi.Model
{
    [Table("News")]
    public class News
    {
        [Key]
        public int NewsID { get; set; }

        [Required]
        [StringLength(10000)]
        public string Title { get; set; }

        public string Content { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Image { get; set; }

        public DateTime PostedDate { get; set; }

        [StringLength(10000)]
        public string Author { get; set; }
    }
} 