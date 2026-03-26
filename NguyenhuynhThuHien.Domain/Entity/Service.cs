namespace NguyenhuynhThuHien.Domain.Entity
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Duration { get; set; } // Tính bằng phút (Ví dụ: 30, 60)

        public decimal Price { get; set; }

        // Navigation property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
