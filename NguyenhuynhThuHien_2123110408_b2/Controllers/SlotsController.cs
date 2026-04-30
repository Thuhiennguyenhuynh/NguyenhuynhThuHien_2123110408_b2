using Microsoft.AspNetCore.Mvc;
using NguyenhuynhThuHien_2123110408_b2.Services;
using System;
using System.Threading.Tasks;

namespace NguyenhuynhThuHien_2123110408_b2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotsController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotsController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSlots([FromQuery] DateTime date, [FromQuery] int dentistId, [FromQuery] int serviceId)
        {
            var slots = await _slotService.GetAvailableSlotsAsync(date, dentistId, serviceId);
            return Ok(slots);
        }
    }
}