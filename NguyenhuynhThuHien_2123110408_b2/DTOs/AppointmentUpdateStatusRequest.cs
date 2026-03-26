using System.ComponentModel.DataAnnotations;

namespace NguyenhuynhThuHien_2123110408_b2.DTOs
{
    public class AppointmentUpdateStatusRequest
    {
        [Required]
        [Range(0, 6, ErrorMessage = "Trạng thái không hợp lệ (Chỉ từ 0 đến 6).")]
        public byte Status { get; set; }
        // 0: Pending, 1: Confirmed, 2: CheckedIn, 3: InTreatment, 4: Completed, 5: Cancelled, 6: NoShow
    }
}
