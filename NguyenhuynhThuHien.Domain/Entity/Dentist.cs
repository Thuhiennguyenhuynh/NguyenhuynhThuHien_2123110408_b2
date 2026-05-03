namespace NguyenhuynhThuHien.Domain.Entity
{
        public class Dentist
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Specialty { get; set; } = string.Empty;

            public byte Status { get; set; } = 1;
            public int? UserId { get; set; }
            public User? User { get; set; }

            // Navigation properties
            public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
            public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        }
    }

