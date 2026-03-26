using NguyenhuynhThuHien.Domain.Entity.NguyenhuynhThuHien_2123110408_b2.Models;

namespace NguyenhuynhThuHien.Domain.Entity
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public int DentistId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Available"; // Available, Booked

        // Navigation property
        public Dentist Dentist { get; set; } = null!;
    }
}
