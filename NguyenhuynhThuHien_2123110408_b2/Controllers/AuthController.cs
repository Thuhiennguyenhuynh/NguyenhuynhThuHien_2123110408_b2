using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NguyenhuynhThuHien.Domain.Data;
using NguyenhuynhThuHien.Domain.Entity;
using NguyenhuynhThuHien_2123110408_b2.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NguyenhuynhThuHien_2123110408_b2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // --- 1. API ĐĂNG KÝ ---
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra xem Username đã tồn tại chưa
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return Conflict(new { Message = "Tên đăng nhập này đã tồn tại!" });
            }

            // Mã hóa mật khẩu người dùng nhập vào
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Lưu thông tin vào Database thật
            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Role = request.Role
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Đăng ký thành công!" });
        }

        // --- 2. API ĐĂNG NHẬP ---
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. Tìm User trong DB
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);

            // Nếu không tìm thấy user HOẶC sai mật khẩu (dùng BCrypt.Verify để so sánh)
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { Message = "Sai tên đăng nhập hoặc mật khẩu" });
            }

            // 2. Tạo Claims (lấy Role TỪ DATABASE, đảm bảo bảo mật tuyệt đối)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // 3. Lấy Key từ appsettings.json để ký Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 4. Tạo Token
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2), // Token sống được 2 tiếng
                signingCredentials: creds
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            // Trả về Token và Role để Frontend (Vue.js) dễ dàng chuyển hướng (chuyển Admin page hoặc Patient page)
            return Ok(new { Token = jwtToken, Role = user.Role, Expires = tokenDescriptor.ValidTo });
        }
    }
}
