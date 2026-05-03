using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenhuynhThuHien.Domain.Constants;
using NguyenhuynhThuHien.Domain.Data;
using NguyenhuynhThuHien.Domain.Entity;
using NguyenhuynhThuHien_2123110408_b2.DTOs;

namespace NguyenhuynhThuHien_2123110408_b2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bắt buộc phải đăng nhập (có token) mới được vào Controller này
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CHỈ ADMIN mới được tạo tài khoản nhân viên
        [HttpPost("create-staff")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (request.Role != AppRoles.Dentist && request.Role != AppRoles.Receptionist)
            {
                return BadRequest(new { Message = "Chỉ được tạo tài khoản cho Dentist hoặc Receptionist." });
            }
            if(request.Role == AppRoles.Dentist && string.IsNullOrEmpty(request.Specialty))
            {
                return BadRequest(new { Message = "Phải cung cấp chuyên môn khi tạo tài khoản Dentist." });
            }

            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return Conflict(new { Message = "Tên đăng nhập này đã tồn tại!" });
            }


            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newUser = new User
                {
                    Username = request.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = request.Role
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                if (request.Role == AppRoles.Dentist)
                {
                    var newDentist = new Dentist
                    {
                        Name = request.Name,
                        UserId = newUser.Id,
                        Specialty = request.Specialty ?? "General Dentistry"
                    };
                    _context.Dentists.Add(newDentist);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return Ok(new { Message = $"Đã tạo thành công tài khoản {request.Role} cho {request.Name}" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "Lỗi hệ thống khi tạo tài khoản", Details = ex.Message });
            }
        }

        // CẢ ADMIN VÀ LỄ TÂN đều được xem thống kê Dashboard
        [HttpGet("dashboard-stats")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                var today = DateTime.Today;

                var appointmentsToday = await _context.Appointments
                    .Where(a => a.StartTime.Date == today)
                    .CountAsync();

                var pendingAppointments = await _context.Appointments
                    .Where(a => a.Status == 0)
                    .CountAsync();

                var activeDentists = await _context.Dentists
                    .Where(d => d.Status == 1)
                    .CountAsync();

                return Ok(new
                {
                    appointmentsToday,
                    pendingAppointments,
                    activeDentists
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi hệ thống khi lấy thống kê", Details = ex.Message });
            }

        }

        // CHỈ ADMIN mới được xem danh sách Lễ tân
        [HttpGet("receptionists")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReceptionists()
        {
            try
            {
                var receptionists = await _context.Users
                    .Where(u => u.Role == AppRoles.Receptionist)
                    .Select(u => new
                    {
                        u.Id,
                        u.Username,
                        u.Role
                    })
                    .ToListAsync();

                return Ok(receptionists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi lấy danh sách lễ tân", Details = ex.Message });
            }
        }

        // CHỈ ADMIN mới được xóa tài khoản Lễ tân
        [HttpDelete("receptionists/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReceptionist(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.Role != AppRoles.Receptionist)
            {
                return NotFound(new { Message = "Không tìm thấy tài khoản Lễ tân này." });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Đã xóa tài khoản Lễ tân thành công!" });
        }
    }
}