using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NguyenhuynhThuHien_2123110408_b2.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace NguyenhuynhThuHien_2123110408_b2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // --- 1. API ĐĂNG KÝ (MÃ HÓA MẬT KHẨU) ---
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request) // Bạn tự tạo DTO RegisterRequest nhé
        {
            // Mã hóa mật khẩu người dùng nhập vào
            // Ví dụ: Nhập "123" -> Hash ra chuỗi "$2a$11$N.G.k0...x"
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // TODO: Lưu thông tin vào Database (Giả lập)
            /*
            var newUser = new User {
                Username = request.Username,
                PasswordHash = passwordHash,
                Role = request.Role
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            */

            return Ok(new { Message = "Đăng ký thành công!", Hash = passwordHash });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // GIẢ LẬP ĐĂNG NHẬP: Bất kỳ ai nhập Password là "123" đều được cho qua
            if (request.Password != "123")
            {
                return Unauthorized(new { Message = "Sai mật khẩu" });
            }

            // 1. Tạo các thông tin đính kèm vào Token (Claims)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, request.Username),
                new Claim(ClaimTypes.Role, request.Role) // Lưu quyền của người này
            };

            // 2. Lấy Key từ appsettings.json để ký Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. Tạo Token
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2), // Token sống được 2 tiếng
                signingCredentials: creds
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return Ok(new { Token = jwtToken, Role = request.Role, Expires = tokenDescriptor.ValidTo });
        }
    }
}
