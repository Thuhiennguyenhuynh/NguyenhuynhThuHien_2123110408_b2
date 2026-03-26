using Microsoft.AspNetCore.Mvc;
using NguyenhuynhThuHien_2123110408_b2.DTOs;
using NguyenhuynhThuHien_2123110408_b2.Services;

namespace NguyenhuynhThuHien_2123110408_b2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChairsController : ControllerBase
    {
        private readonly IChairService _chairService;
        public ChairsController(IChairService chairService) => _chairService = chairService;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _chairService.GetAllChairsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var c = await _chairService.GetChairByIdAsync(id);
            return c == null ? NotFound() : Ok(c);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChairRequest request)
        {
            var c = await _chairService.CreateChairAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ChairRequest request)
            => await _chairService.UpdateChairAsync(id, request) ? Ok(new { Message = "Thành công" }) : NotFound();

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try { return await _chairService.DeleteChairAsync(id) ? Ok(new { Message = "Đã xóa" }) : NotFound(); }
            catch (InvalidOperationException ex) { return BadRequest(new { Error = ex.Message }); }
        }
    }
}
