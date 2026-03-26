using System.ComponentModel.DataAnnotations;

namespace NguyenhuynhThuHien_2123110408_b2.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // Truyền "Admin", "Receptionist", "Patient" hoặc "Dentist"
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
