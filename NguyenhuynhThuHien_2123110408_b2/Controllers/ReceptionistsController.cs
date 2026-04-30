using Microsoft.AspNetCore.Mvc;
using NguyenhuynhThuHien.Domain.Entity;
using NguyenhuynhThuHien_2123110408_b2.Services;

namespace NguyenhuynhThuHien_2123110408_b2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionistsController : ControllerBase
    {
        private readonly IReceptionistService _receptionistService;

        public ReceptionistsController(IReceptionistService receptionistService)
        {
            _receptionistService = receptionistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _receptionistService.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _receptionistService.GetByIdAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Receptionist receptionist)
        {
            var created = await _receptionistService.CreateAsync(receptionist);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Receptionist receptionist)
        {
            var updated = await _receptionistService.UpdateAsync(id, receptionist);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _receptionistService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}