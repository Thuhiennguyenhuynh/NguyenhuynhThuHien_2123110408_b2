namespace NguyenhuynhThuHien.Domain.Entity
{
    public class Chair
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Status { get; set; } = 1; // 1: Hoạt động, 0: Tạm ngưng

        // Navigation property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
