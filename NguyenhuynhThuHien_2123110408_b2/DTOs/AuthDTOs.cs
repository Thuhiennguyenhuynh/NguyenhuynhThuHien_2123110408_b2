//using System.ComponentModel.DataAnnotations;

//namespace NguyenhuynhThuHien_2123110408_b2.DTOs
//{
//    public class LoginRequest
//    {
//        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
//        [MinLength(3, ErrorMessage = "Tên đăng nhập quá ngắn (tối thiểu 3 ký tự)")]
//        [MaxLength(20, ErrorMessage = "Tên đăng nhập quá dài (tối đa 20 ký tự)")]
//        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập không được chứa ký tự đặc biệt hoặc khoảng trắng")]
//        public string Username { get; set; } = string.Empty;

//        [Required(ErrorMessage = "Mật khẩu không được để trống")]
//        [MinLength(6, ErrorMessage = "Mật khẩu quá ngắn (tối thiểu 6 ký tự)")]
//        [MaxLength(50, ErrorMessage = "Mật khẩu quá dài (tối đa 50 ký tự)")]
//        public string Password { get; set; } = string.Empty;
//    }

//    public class RegisterRequest
//    {
//        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
//        [MinLength(3, ErrorMessage = "Tên đăng nhập quá ngắn (tối thiểu 3 ký tự)")]
//        [MaxLength(20, ErrorMessage = "Tên đăng nhập quá dài (tối đa 20 ký tự)")]
//        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập không được chứa ký tự đặc biệt hoặc khoảng trắng")]
//        public string Username { get; set; } = string.Empty;

//        [Required(ErrorMessage = "Mật khẩu không được để trống")]
//        [MinLength(6, ErrorMessage = "Mật khẩu quá ngắn (tối thiểu 6 ký tự)")]
//        [MaxLength(50, ErrorMessage = "Mật khẩu quá dài (tối đa 50 ký tự)")]
//        public string Password { get; set; } = string.Empty;

//        [Required(ErrorMessage = "Họ và tên không được để trống")]
//        [MinLength(2, ErrorMessage = "Họ và tên quá ngắn")]
//        [MaxLength(50, ErrorMessage = "Họ và tên quá dài")]
//        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Họ và tên chỉ được chứa chữ cái, không chứa số hoặc ký tự đặc biệt")]
//        public string Name { get; set; } = string.Empty;

//        [Required(ErrorMessage = "Số điện thoại không được để trống")]
//        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Số điện thoại không hợp lệ (Phải là số Việt Nam gồm 10 chữ số)")]
//        public string Phone { get; set; } = string.Empty;
//    }

//    public class CreateStaffRequest
//    {
//        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
//        [MinLength(3, ErrorMessage = "Tên đăng nhập quá ngắn (tối thiểu 3 ký tự)")]
//        [MaxLength(20, ErrorMessage = "Tên đăng nhập quá dài (tối đa 20 ký tự)")]
//        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập không được chứa ký tự đặc biệt hoặc khoảng trắng")]
//        public string Username { get; set; } = string.Empty;

//        [Required(ErrorMessage = "Mật khẩu không được để trống")]
//        [MinLength(6, ErrorMessage = "Mật khẩu quá ngắn (tối thiểu 6 ký tự)")]
//        [MaxLength(50, ErrorMessage = "Mật khẩu quá dài (tối đa 50 ký tự)")]
//        public string Password { get; set; } = string.Empty;

//        [Required(ErrorMessage = "Quyền (Role) không được để trống")]
//        public string Role { get; set; } = string.Empty;

//        [Required(ErrorMessage = "Họ và tên nhân viên không được để trống")]
//        [MinLength(2, ErrorMessage = "Họ và tên quá ngắn")]
//        [MaxLength(50, ErrorMessage = "Họ và tên quá dài")]
//        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Họ và tên chỉ được chứa chữ cái, không chứa số hoặc ký tự đặc biệt")]
//        public string Name { get; set; } = string.Empty;

//        // Specialty (Chuyên khoa) chỉ bắt buộc nếu Role là Dentist
//        [MaxLength(100, ErrorMessage = "Chuyên khoa không được vượt quá 100 ký tự")]
//        public string? Specialty { get; set; }


//    }


//}


using System.ComponentModel.DataAnnotations;
using NguyenhuynhThuHien.Domain.Constants;

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

    // ĐÃ SỬA LỖI LỒNG CLASS: Gộp chung thành 1 class duy nhất và kế thừa IValidatableObject
    public class CreateStaffRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        // Regex hỗ trợ Tiếng Việt, độ dài từ 2 đến 50 ký tự
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]{2,50}$", ErrorMessage = "Họ tên chỉ chứa chữ cái, không chứa số/kí tự đặc biệt và dài 2-50 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        // Regex chỉ cho phép chữ cái không dấu, số, dấu gạch dưới, 4-20 ký tự
        [RegularExpression(@"^[a-zA-Z0-9_]{4,20}$", ErrorMessage = "Tên đăng nhập từ 4-20 ký tự, không chứa khoảng trắng hoặc dấu")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vai trò không được để trống")]
        [RegularExpression($"^({AppRoles.Dentist}|{AppRoles.Receptionist})$", ErrorMessage = "Vai trò không hợp lệ (Chỉ nhận Dentist hoặc Receptionist)")]
        public string Role { get; set; } = string.Empty;

        // Specialty không đặt [Required] ở đây vì Lễ tân không cần
        [MaxLength(100, ErrorMessage = "Chuyên khoa không được vượt quá 100 ký tự")]
        public string? Specialty { get; set; }

        // Hàm này sẽ tự động chạy sau khi các Attribute ở trên pass
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Kiểm tra validate CÓ ĐIỀU KIỆN
            if (Role == AppRoles.Dentist)
            {
                if (string.IsNullOrWhiteSpace(Specialty))
                {
                    yield return new ValidationResult(
                        "Vui lòng nhập chuyên khoa cho Nha sĩ",
                        new[] { nameof(Specialty) }
                    );
                }
                else if (Specialty.Length < 2)
                {
                    yield return new ValidationResult(
                        "Chuyên khoa quá ngắn (tối thiểu 2 ký tự)",
                        new[] { nameof(Specialty) }
                    );
                }
            }
        }
    }
}