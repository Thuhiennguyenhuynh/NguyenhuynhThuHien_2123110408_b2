using System.ComponentModel.DataAnnotations;

namespace NguyenhuynhThuHien_2123110408_b2.DTOs
{
    // DTO để hứng dữ liệu khi tạo mới / cập nhật
    public class PatientRequest
    {
        [Required(ErrorMessage = "Tên bệnh nhân không được để trống")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15)]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }
    }

    // DTO để trả dữ liệu ra cho Client
    public class PatientResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
