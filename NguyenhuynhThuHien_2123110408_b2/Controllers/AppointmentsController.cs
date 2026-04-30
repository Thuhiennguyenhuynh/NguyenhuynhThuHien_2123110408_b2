using Microsoft.AspNetCore.Mvc;
using NguyenhuynhThuHien_2123110408_b2.DTOs;
using NguyenhuynhThuHien_2123110408_b2.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

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
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatient(int patientId)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsByPatientIdAsync(patientId);
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _appointmentService.CreateAppointmentAsync(request);
                if (result) return Ok(new { Message = "Đặt lịch thành công!" });
                return BadRequest(new { Message = "Đặt lịch thất bại." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // ==========================================
        // CẬP NHẬT: API THEO CHUẨN SRS 4.5[cite: 4]
        // ==========================================

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                var result = await _appointmentService.CancelAppointmentAsync(id);
                if (result) return Ok(new { Message = "Hủy lịch thành công!" });
                return BadRequest(new { Message = "Hủy lịch thất bại." });
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

        [Authorize(Roles = "Receptionist,Admin")]
        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmAppointment(int id)
        {
            try
            {
                // Trạng thái 1 = Confirmed[cite: 4]
                var result = await _appointmentService.UpdateAppointmentStatusAsync(id, 1);
                if (result) return Ok(new { Message = "Xác nhận lịch thành công!" });
                return BadRequest(new { Message = "Xác nhận lịch thất bại." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [Authorize(Roles = "Receptionist,Admin")]
        [HttpPut("{id}/checkin")]
        public async Task<IActionResult> CheckInAppointment(int id)
        {
            try
            {
                // Gọi thuật toán Gán ghế tự động khi Check-in[cite: 4]
                var result = await _appointmentService.CheckInAsync(id);
                if (result) return Ok(new { Message = "Check-in bệnh nhân thành công và đã gán ghế!" });
                return BadRequest(new { Message = "Check-in thất bại." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // ==========================================
        // CÁC HÀM GET KHÁC
        // ==========================================

        [HttpGet("/api/slots")]
        public async Task<IActionResult> GetTimeSlots([FromQuery] int dentistId, [FromQuery] DateTime date, [FromQuery] int serviceId)
        {
            try
            {
                if (date.Date < DateTime.Now.Date)
                {
                    return BadRequest(new { Message = "Không thể tra cứu giờ trống trong quá khứ." });
                }

                // Đã truyền đủ 3 tham số (thêm serviceId) để khớp với Service[cite: 4]
                var slots = await _appointmentService.GetAvailableTimeSlotsAsync(dentistId, serviceId, date);

                return Ok(slots);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpGet("dentist/{dentistId}")]
        public async Task<IActionResult> GetAppointmentsByDentist(int dentistId)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsByDentistIdAsync(dentistId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}