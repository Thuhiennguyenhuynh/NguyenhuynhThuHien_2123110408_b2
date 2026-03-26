using System.ComponentModel.DataAnnotations;

namespace NguyenhuynhThuHien_2123110408_b2.DTOs
{
    public class ChairRequest
    {
        [Required(ErrorMessage = "Tên ghế không được để trống")]
        public string Name { get; set; } = string.Empty;
        public byte Status { get; set; } = 1; // 1: Hoạt động, 0: Bảo trì
    }
    public class ChairResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Status { get; set; }
    }
}
