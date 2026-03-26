using Microsoft.AspNetCore.Mvc;
using NguyenhuynhThuHien_2123110408_b2.DTOs;
using NguyenhuynhThuHien_2123110408_b2.Services;

namespace NguyenhuynhThuHien_2123110408_b2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistsController : ControllerBase
    {
        private readonly IDentistService _dentistService;

        public DentistsController(IDentistService dentistService)
        {
            _dentistService = dentistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dentists = await _dentistService.GetAllDentistsAsync();
            return Ok(dentists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dentist = await _dentistService.GetDentistByIdAsync(id);
            if (dentist == null) return NotFound(new { Message = "Không tìm thấy nha sĩ." });
            return Ok(dentist);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DentistRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newDentist = await _dentistService.CreateDentistAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newDentist.Id }, newDentist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DentistRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _dentistService.UpdateDentistAsync(id, request);
            if (!result) return NotFound(new { Message = "Không tìm thấy nha sĩ." });

            return Ok(new { Message = "Cập nhật thành công!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _dentistService.DeleteDentistAsync(id);
                if (!result) return NotFound(new { Message = "Không tìm thấy nha sĩ." });

                return Ok(new { Message = "Xóa nha sĩ thành công!" });
            }
            catch (InvalidOperationException ex)
            {
                // Bắt lỗi khi nha sĩ đã có lịch khám
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
