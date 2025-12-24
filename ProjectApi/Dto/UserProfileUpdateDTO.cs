using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Dto
{
    public class UserProfileUpdateDTO
    {
        [Required(ErrorMessage = "Email không được để trống.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên của bạn .")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = " Vui lòng nhập ngày tháng năm sinh của bạn.")]
        public DateTime? DateBirth { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại của bạn .")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng lựa chọn giới tính .")]
        public int Gender { get; set; }

        // Image is optional for update
        public string? Image { get; set; }
    }
} 