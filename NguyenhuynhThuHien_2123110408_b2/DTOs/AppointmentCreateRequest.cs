using System.ComponentModel.DataAnnotations;

namespace NguyenhuynhThuHien_2123110408_b2.DTOs
{
    public class AppointmentCreateRequest
    {
        [Required(ErrorMessage = "Vui lòng chọn Bệnh nhân")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Nha sĩ")]
        public int DentistId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Ghế khám")]
        public int ChairId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Dịch vụ")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Thời gian bắt đầu")]
        public DateTime StartTime { get; set; }

        // Không cần EndTime vì hệ thống sẽ tự tính toán dựa vào Service Duration
    }
}
