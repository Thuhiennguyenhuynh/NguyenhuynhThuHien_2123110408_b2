namespace NguyenhuynhThuHien.Domain.Entity
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
