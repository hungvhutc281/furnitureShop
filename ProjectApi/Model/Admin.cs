using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Model
{
  
        [Table("Admin")]
        public class Admin
        {
            [Key]
            public int  AdminId { get; set; }

            [Required, MaxLength(1000)]
            public string Name { get; set; }

            [Required, MaxLength(1000)]
            public string Address { get; set; }

            [Required, MaxLength(15)]
            public string Phone { get; set; }

            [Required, MaxLength(30)]
            public string userAdmin { get; set; }

            [Required, MaxLength(255)]
            public string passwordAdmin { get; set; }

            [Required, MaxLength(200)]
            public string Avatar { get; set; }

            [Required, MaxLength(50)]
            public string Email { get; set; }

            // Navigation Property
            [ForeignKey("Role")]
            public int RoleId { get; set; }
            public Role Role { get; set; }
        }
    }

