using System.ComponentModel.DataAnnotations;

namespace NguyenhuynhThuHien_2123110408_b2.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quyền (Role) không được để trống")]
        public string Role { get; set; } = "Patient"; // Gợi ý Role mặc định
    }

    public class CreateStaffRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quyền (Role) không được để trống")]
        public string Role { get; set; } = string.Empty; // Sẽ truyền "Dentist" hoặc "Receptionist"

        [Required(ErrorMessage = "Họ và tên nhân viên không được để trống")]
        public string Name { get; set; } = string.Empty;

        // Specialty (Chuyên khoa) chỉ bắt buộc nếu Role là Dentist
        public string? Specialty { get; set; }
    }
}
