using Microsoft.EntityFrameworkCore;
using NguyenhuynhThuHien.Domain.Data;
using NguyenhuynhThuHien.Domain.Entity;
using NguyenhuynhThuHien_2123110408_b2.DTOs;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public class DentalService : IDentalService
    {
        private readonly ApplicationDbContext _context;

        public DentalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceResponse>> GetAllServicesAsync()
        {
            return await _context.Services.Select(s => new ServiceResponse
            {
                Id = s.Id,
                Name = s.Name,
                Duration = s.Duration,
                Price = s.Price
            }).ToListAsync();
        }

        public async Task<ServiceResponse?> GetServiceByIdAsync(int id)
        {
            var s = await _context.Services.FindAsync(id);
            if (s == null) return null;
            return new ServiceResponse { Id = s.Id, Name = s.Name, Duration = s.Duration, Price = s.Price };
        }

        public async Task<ServiceResponse> CreateServiceAsync(ServiceRequest request)
        {
            var newService = new Service { Name = request.Name, Duration = request.Duration, Price = request.Price };
            _context.Services.Add(newService);
            await _context.SaveChangesAsync();
            return new ServiceResponse { Id = newService.Id, Name = newService.Name, Duration = newService.Duration, Price = newService.Price };
        }

        public async Task<bool> UpdateServiceAsync(int id, ServiceRequest request)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            service.Name = request.Name; service.Duration = request.Duration; service.Price = request.Price;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            if (await _context.Appointments.AnyAsync(a => a.ServiceId == id))
                throw new InvalidOperationException("Không thể xóa dịch vụ đã có người đặt lịch.");

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
