using ProjectApi.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectApi.Dto
{
    public class UserDTO
    {
        public int? UserId { get; set; }
        [Required(ErrorMessage = "Email không được để trống.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên của bạn .")]
        public string FullName { get; set; }

        [Required(ErrorMessage = " Vui lòng nhập ngày tháng năm sinh của bạn.")]

        public DateTime? DateBirth { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại của bạn .")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password không được để trống.")]
        public string Password { get; set; }
        public string? Image { get; set; }

        //[JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = "Vui lòng lựa chọn giới tính .")]
        public GenderType Gender { get; set; } = GenderType.Nam;


    }
   

    public class UserProfileDTO
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Email không được để trống.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên của bạn .")]
        public string FullName { get; set; }

        [Required(ErrorMessage = " Vui lòng nhập ngày tháng năm sinh của bạn.")]

        public DateTime? DateBirth { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại của bạn .")]
        public string PhoneNumber { get; set; }

        public string? Image { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        //[JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = "Vui lòng lựa chọn giới tính .")]
        public GenderType Gender { get; set; } = GenderType.Nam;
    }
}
