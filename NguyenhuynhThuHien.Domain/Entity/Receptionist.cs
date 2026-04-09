namespace NguyenhuynhThuHien.Domain.Entity
{
    public class Receptionist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; } // Thêm SĐT để quản lý

        // Khóa ngoại liên kết với bảng User
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
