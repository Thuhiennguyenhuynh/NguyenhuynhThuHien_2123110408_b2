using Microsoft.AspNetCore.Mvc;
using NguyenhuynhThuHien_2123110408_b2.DTOs;
using NguyenhuynhThuHien_2123110408_b2.Services;

namespace NguyenhuynhThuHien_2123110408_b2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound(new { Message = "Không tìm thấy bệnh nhân." });
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newPatient = await _patientService.CreatePatientAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = newPatient.Id }, newPatient);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message }); // Lỗi trùng SDT
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _patientService.UpdatePatientAsync(id, request);
                if (!result) return NotFound(new { Message = "Không tìm thấy bệnh nhân." });
                return Ok(new { Message = "Cập nhật thành công!" });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _patientService.DeletePatientAsync(id);
                if (!result) return NotFound(new { Message = "Không tìm thấy bệnh nhân." });
                return Ok(new { Message = "Xóa bệnh nhân thành công!" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message }); // Lỗi dính khóa ngoại (đã có lịch khám)
            }
        }
    }
}
