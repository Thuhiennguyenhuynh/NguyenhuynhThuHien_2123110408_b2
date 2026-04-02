using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenhuynhThuHien.Domain.Constants;
using NguyenhuynhThuHien.Domain.Data;
using NguyenhuynhThuHien.Domain.Entity;
using NguyenhuynhThuHien.Domain.Entity.NguyenhuynhThuHien_2123110408_b2.Models;
using NguyenhuynhThuHien_2123110408_b2.DTOs;

namespace NguyenhuynhThuHien_2123110408_b2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // BẢO MẬT: Chỉ user có Role "Admin" mới được truy cập các API trong Controller này
    [Authorize(Roles = AppRoles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("create-staff")]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // 1. Kiểm tra Role hợp lệ (Không cho phép tạo thêm Admin hoặc tạo Patient ở đây)
            if (request.Role != AppRoles.Dentist && request.Role != AppRoles.Receptionist)
            {
                return BadRequest(new { Message = "Chỉ được tạo tài khoản cho Dentist hoặc Receptionist." });
            }

            // 2. Kiểm tra trùng lặp Username
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return Conflict(new { Message = "Tên đăng nhập này đã tồn tại!" });
            }

            // 3. Dùng Transaction vì ta cập nhật nhiều bảng cùng lúc
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Tạo User account
                var newUser = new User
                {
                    Username = request.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = request.Role
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Nếu là Nha sĩ (Dentist), phải tạo thêm hồ sơ trong bảng Dentists
                if (request.Role == AppRoles.Dentist)
                {
                    var newDentist = new Dentist
                    {
                        Name = request.Name,
                        Specialty = request.Specialty ?? "General Dentistry" // Mặc định nếu không truyền
                        // Nếu entity Dentist của bạn có cột UserId thì gán: UserId = newUser.Id
                    };
                    _context.Dentists.Add(newDentist);
                    await _context.SaveChangesAsync();
                }

                // (Tùy chọn) Nếu sau này bạn có bảng Receptionist thì thêm if (Role == Receptionist) ở đây

                await transaction.CommitAsync();
                return Ok(new { Message = $"Đã tạo thành công tài khoản {request.Role} cho {request.Name}" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "Lỗi hệ thống khi tạo tài khoản", Details = ex.Message });
            }
        }
    }
}