using Microsoft.EntityFrameworkCore;
using NguyenhuynhThuHien.Domain.Data;
using NguyenhuynhThuHien.Domain.Entity;
using NguyenhuynhThuHien_2123110408_b2.DTOs;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public class ChairService : IChairService
    {
        private readonly ApplicationDbContext _context;
        public ChairService(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<ChairResponse>> GetAllChairsAsync() =>
            await _context.Chairs.Select(c => new ChairResponse { Id = c.Id, Name = c.Name, Status = c.Status }).ToListAsync();

        public async Task<ChairResponse?> GetChairByIdAsync(int id)
        {
            var c = await _context.Chairs.FindAsync(id);
            return c == null ? null : new ChairResponse { Id = c.Id, Name = c.Name, Status = c.Status };
        }

        public async Task<ChairResponse> CreateChairAsync(ChairRequest request)
        {
            var chair = new Chair { Name = request.Name, Status = request.Status };
            _context.Chairs.Add(chair); await _context.SaveChangesAsync();
            return new ChairResponse { Id = chair.Id, Name = chair.Name, Status = chair.Status };
        }

        public async Task<bool> UpdateChairAsync(int id, ChairRequest request)
        {
            var chair = await _context.Chairs.FindAsync(id);
            if (chair == null) return false;
            chair.Name = request.Name; chair.Status = request.Status;
            await _context.SaveChangesAsync(); return true;
        }

        public async Task<bool> DeleteChairAsync(int id)
        {
            var chair = await _context.Chairs.FindAsync(id);
            if (chair == null) return false;
            if (await _context.Appointments.AnyAsync(a => a.ChairId == id))
                throw new InvalidOperationException("Không thể xóa ghế đã có lịch đặt. Hãy đổi Status = 0 (Bảo trì).");
            _context.Chairs.Remove(chair); await _context.SaveChangesAsync(); return true;
        }
    }
}
