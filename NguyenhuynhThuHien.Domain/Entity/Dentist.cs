namespace NguyenhuynhThuHien.Domain.Entity
{
    namespace NguyenhuynhThuHien_2123110408_b2.Models
    {
        public class Dentist
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Specialty { get; set; } = string.Empty;

            public byte Status { get; set; } = 1;

            // Navigation properties
            public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
            public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        }
    }
}
