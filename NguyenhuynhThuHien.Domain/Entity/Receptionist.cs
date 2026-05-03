namespace NguyenhuynhThuHien.Domain.Entity
{
    public class Receptionist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; } // SRS: Nullable - SĐT liên lạc nội bộ

        // Khóa ngoại liên kết 1-1 với bảng User (NOT NULL, UNIQUE theo SRS)
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
