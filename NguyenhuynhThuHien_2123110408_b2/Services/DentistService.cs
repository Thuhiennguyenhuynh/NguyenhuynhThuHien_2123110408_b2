using Microsoft.EntityFrameworkCore;
using NguyenhuynhThuHien.Domain.Data;
using NguyenhuynhThuHien.Domain.Entity;
using NguyenhuynhThuHien_2123110408_b2.DTOs;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public class DentistService : IDentistService
    {
        private readonly ApplicationDbContext _context;

        public DentistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DentistResponse>> GetAllDentistsAsync()
        {
            return await _context.Dentists
                .Select(d => new DentistResponse
                {
                    Id = d.Id,
                    Name = d.Name,
                    Specialty = d.Specialty,
                    Status = d.Status
                }).ToListAsync();
        }

        public async Task<DentistResponse?> GetDentistByIdAsync(int id)
        {
            var dentist = await _context.Dentists.FindAsync(id);
            if (dentist == null) return null;

            return new DentistResponse
            {
                Id = dentist.Id,
                Name = dentist.Name,
                Specialty = dentist.Specialty,
                Status = dentist.Status
            };
        }

        public async Task<DentistResponse> CreateDentistAsync(DentistRequest request)
        {
            var newDentist = new Dentist
            {
                Name = request.Name,
                Specialty = request.Specialty,
                Status = request.Status
            };

            _context.Dentists.Add(newDentist);
            await _context.SaveChangesAsync();

            return new DentistResponse
            {
                Id = newDentist.Id,
                Name = newDentist.Name,
                Specialty = newDentist.Specialty,
                Status = newDentist.Status
            };
        }

        public async Task<bool> UpdateDentistAsync(int id, DentistRequest request)
        {
            var dentist = await _context.Dentists.FindAsync(id);
            if (dentist == null) return false;

            dentist.Name = request.Name;
            dentist.Specialty = request.Specialty;
            dentist.Status = request.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDentistAsync(int id)
        {
            var dentist = await _context.Dentists.FindAsync(id);
            if (dentist == null) return false;

            // Ràng buộc: Không cho xóa nha sĩ nếu họ đã có lịch khám
            var hasAppointments = await _context.Appointments.AnyAsync(a => a.DentistId == id);
            if (hasAppointments)
                throw new InvalidOperationException("Không thể xóa nha sĩ đã có lịch khám. Gợi ý: Hãy đổi Status thành 0 (Tạm ngưng) thay vì xóa.");

            _context.Dentists.Remove(dentist);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
