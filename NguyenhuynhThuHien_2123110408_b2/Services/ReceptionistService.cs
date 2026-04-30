using Microsoft.EntityFrameworkCore;
using NguyenhuynhThuHien.Domain.Data;
using NguyenhuynhThuHien.Domain.Entity;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public class ReceptionistService : IReceptionistService
    {
        private readonly ApplicationDbContext _context;

        public ReceptionistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Receptionist>> GetAllAsync()
        {
            return await _context.Set<Receptionist>().ToListAsync();
        }

        public async Task<Receptionist> GetByIdAsync(int id)
        {
            return await _context.Set<Receptionist>().FindAsync(id);
        }

        public async Task<Receptionist> CreateAsync(Receptionist receptionist)
        {
            _context.Set<Receptionist>().Add(receptionist);
            await _context.SaveChangesAsync();
            return receptionist;
        }

        public async Task<Receptionist> UpdateAsync(int id, Receptionist receptionist)
        {
            var existing = await _context.Set<Receptionist>().FindAsync(id);
            if (existing == null) return null;

            existing.Name = receptionist.Name;
            existing.Phone = receptionist.Phone;
            // Cập nhật thêm các trường cần thiết khác

            _context.Set<Receptionist>().Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Set<Receptionist>().FindAsync(id);
            if (existing == null) return false;

            _context.Set<Receptionist>().Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
