using System.ComponentModel.DataAnnotations;

namespace NguyenhuynhThuHien_2123110408_b2.DTOs
{
    public class ServiceRequest
    {
        [Required(ErrorMessage = "Tên dịch vụ không được để trống")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 300, ErrorMessage = "Thời lượng (phút) phải từ 1 đến 300")]
        public int Duration { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá tiền không hợp lệ")]
        public decimal Price { get; set; }
    }

    public class ServiceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Duration { get; set; }
        public decimal Price { get; set; }
    }
}
