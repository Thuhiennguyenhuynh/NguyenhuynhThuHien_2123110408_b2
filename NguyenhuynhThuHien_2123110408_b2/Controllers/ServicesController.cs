using Microsoft.AspNetCore.Mvc;
using NguyenhuynhThuHien_2123110408_b2.DTOs;
using NguyenhuynhThuHien_2123110408_b2.Services;

namespace NguyenhuynhThuHien_2123110408_b2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IDentalService _dentalService;
        public ServicesController(IDentalService dentalService) => _dentalService = dentalService;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _dentalService.GetAllServicesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var s = await _dentalService.GetServiceByIdAsync(id);
            return s == null ? NotFound() : Ok(s);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceRequest request)
        {
            var s = await _dentalService.CreateServiceAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = s.Id }, s);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceRequest request)
            => await _dentalService.UpdateServiceAsync(id, request) ? Ok(new { Message = "Thành công" }) : NotFound();

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try { return await _dentalService.DeleteServiceAsync(id) ? Ok(new { Message = "Đã xóa" }) : NotFound(); }
            catch (InvalidOperationException ex) { return BadRequest(new { Error = ex.Message }); }
        }
    }
}
