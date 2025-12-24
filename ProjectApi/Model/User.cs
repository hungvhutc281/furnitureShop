using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace ProjectApi.Model
{
    public enum GenderType
    {
        Nam,           // Nam
        Nữ,         // Nữ
        Riêngtư     // Không muốn tiết lộ
    } 
    // Bảng Người dùng (Quản lý tài khoản)
    [Table("User")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, StringLength(1000)]
        public string FullName { get; set; }

        [Required, StringLength(1000), EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(2550)]
        public string Address { get; set; }

        [Required, StringLength(255)]
        public string Password { get; set; }

        public DateTime? DateBirth { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(200)]
        public string? Image { get; set; }


        [StringLength(6)]
        public string? OtpCode { get; set; }

        public DateTime? OtpExpiryTime { get; set; }

        public string? GoogleId { get; set; } //Mã định danh khách hàng trên Google
        public GenderType Gender { get; set; } = GenderType.Nam;
        [ForeignKey("Role")]
        public int? RoleId { get; set; }
        public Role? Role { get; set; }

        public void HashPassword()
        {
            using (var sha256 = SHA256.Create())
            {
                Password = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(Password)));
            }
        }
    }

}
