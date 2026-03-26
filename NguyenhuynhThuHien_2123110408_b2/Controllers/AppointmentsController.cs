using Microsoft.AspNetCore.Mvc;
using NguyenhuynhThuHien_2123110408_b2.DTOs;
using NguyenhuynhThuHien_2123110408_b2.Services;
using Microsoft.AspNetCore.Authorization;

namespace NguyenhuynhThuHien_2123110408_b2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            try
            {
                var appointments = await _appointmentService.GetAllAppointmentsAsync();

                // Nếu không có dữ liệu, trả về danh sách rỗng 200 OK
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _appointmentService.CreateAppointmentAsync(request);
                if (result)
                {
                    return Ok(new { Message = "Đặt lịch thành công!" });
                }
                return BadRequest(new { Message = "Đặt lịch thất bại." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message }); // Trả về mã 409 Conflict nếu trùng lịch
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Lỗi hệ thống: " + ex.Message });
            }
        }


        // Nếu muốn chỉ Lễ tân hoặc Admin mới được cập nhật trạng thái lịch, thêm dòng này:
        [Authorize(Roles = "Receptionist,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, [FromBody] AppointmentUpdateStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _appointmentService.UpdateAppointmentStatusAsync(id, request.Status);
                if (result)
                {
                    return Ok(new { Message = "Cập nhật trạng thái thành công!" });
                }
                return BadRequest(new { Message = "Cập nhật thất bại." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                var result = await _appointmentService.CancelAppointmentAsync(id);
                if (result)
                {
                    return Ok(new { Message = "Hủy lịch thành công!" });
                }
                return BadRequest(new { Message = "Hủy lịch thất bại." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Error = ex.Message }); // 404 Không tìm thấy
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message }); // 400 Lỗi vi phạm rule 2 tiếng
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpGet("timeslots")]
        public async Task<IActionResult> GetTimeSlots([FromQuery] int dentistId, [FromQuery] DateTime date)
        {
            try
            {
                // Validate nếu người dùng truyền ngày trong quá khứ
                if (date.Date < DateTime.Now.Date)
                {
                    return BadRequest(new { Message = "Không thể tra cứu giờ trống trong quá khứ." });
                }

                var slots = await _appointmentService.GetAvailableTimeSlotsAsync(dentistId, date);

                return Ok(new
                {
                    DentistId = dentistId,
                    Date = date.ToString("yyyy-MM-dd"),
                    AvailableSlots = slots
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
