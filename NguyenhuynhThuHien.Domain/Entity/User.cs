
namespace NguyenhuynhThuHien.Domain.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Phân quyền theo SRS: "Admin", "Patient", "Dentist", "Receptionist"
        public string Role { get; set; } = "Patient";
    }
}
