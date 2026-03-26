using System.ComponentModel.DataAnnotations;

namespace NguyenhuynhThuHien_2123110408_b2.DTOs
{
    // DTO hứng dữ liệu từ Request
    public class DentistRequest
    {
        [Required(ErrorMessage = "Tên nha sĩ không được để trống")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Chuyên khoa không được để trống")]
        [MaxLength(255)]
        public string Specialty { get; set; } = string.Empty;

        // Trạng thái: 1 là Đang làm việc, 0 là Tạm ngưng
        public byte Status { get; set; } = 1;
    }

    // DTO trả dữ liệu ra Response
    public class DentistResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public byte Status { get; set; }
    }
}
