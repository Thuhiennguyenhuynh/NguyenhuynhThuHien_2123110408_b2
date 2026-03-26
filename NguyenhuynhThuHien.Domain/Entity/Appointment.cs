using NguyenhuynhThuHien.Domain.Entity.NguyenhuynhThuHien_2123110408_b2.Models;

namespace NguyenhuynhThuHien.Domain.Entity
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DentistId { get; set; }
        public int ServiceId { get; set; }

        public int ChairId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // Theo business rule: "Đánh dấu trạng thái NoShow", có thể lưu trạng thái ở đây (Pending, Confirmed, Cancelled, NoShow)
        // (Ví dụ: 0: Pending, 1: Confirmed, 2: CheckedIn, 3: InTreatment, 4: Completed, 5: Cancelled, 6: NoShow)
        public byte Status { get; set; } = 0;

        // Navigation properties
        public Patient Patient { get; set; } = null!;
        public Dentist Dentist { get; set; } = null!;
        public Service Service { get; set; } = null!;
        public Chair Chair { get; set; } = null!; // Thêm Navigation property cho Chair
    }

}
