using System.ComponentModel.DataAnnotations;

namespace NguyenhuynhThuHien_2123110408_b2.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [MinLength(3, ErrorMessage = "Tên đăng nhập quá ngắn (tối thiểu 3 ký tự)")]
        [MaxLength(20, ErrorMessage = "Tên đăng nhập quá dài (tối đa 20 ký tự)")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập không được chứa ký tự đặc biệt hoặc khoảng trắng")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu quá ngắn (tối thiểu 6 ký tự)")]
        [MaxLength(50, ErrorMessage = "Mật khẩu quá dài (tối đa 50 ký tự)")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [MinLength(3, ErrorMessage = "Tên đăng nhập quá ngắn (tối thiểu 3 ký tự)")]
        [MaxLength(20, ErrorMessage = "Tên đăng nhập quá dài (tối đa 20 ký tự)")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập không được chứa ký tự đặc biệt hoặc khoảng trắng")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu quá ngắn (tối thiểu 6 ký tự)")]
        [MaxLength(50, ErrorMessage = "Mật khẩu quá dài (tối đa 50 ký tự)")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MinLength(2, ErrorMessage = "Họ và tên quá ngắn")]
        [MaxLength(50, ErrorMessage = "Họ và tên quá dài")]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Họ và tên chỉ được chứa chữ cái, không chứa số hoặc ký tự đặc biệt")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Số điện thoại không hợp lệ (Phải là số Việt Nam gồm 10 chữ số)")]
        public string Phone { get; set; } = string.Empty;
    }

    public class CreateStaffRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [MinLength(3, ErrorMessage = "Tên đăng nhập quá ngắn (tối thiểu 3 ký tự)")]
        [MaxLength(20, ErrorMessage = "Tên đăng nhập quá dài (tối đa 20 ký tự)")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập không được chứa ký tự đặc biệt hoặc khoảng trắng")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu quá ngắn (tối thiểu 6 ký tự)")]
        [MaxLength(50, ErrorMessage = "Mật khẩu quá dài (tối đa 50 ký tự)")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quyền (Role) không được để trống")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên nhân viên không được để trống")]
        [MinLength(2, ErrorMessage = "Họ và tên quá ngắn")]
        [MaxLength(50, ErrorMessage = "Họ và tên quá dài")]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Họ và tên chỉ được chứa chữ cái, không chứa số hoặc ký tự đặc biệt")]
        public string Name { get; set; } = string.Empty;

        // Specialty (Chuyên khoa) chỉ bắt buộc nếu Role là Dentist
        [MaxLength(100, ErrorMessage = "Chuyên khoa không được vượt quá 100 ký tự")]
        public string? Specialty { get; set; }
    }
}