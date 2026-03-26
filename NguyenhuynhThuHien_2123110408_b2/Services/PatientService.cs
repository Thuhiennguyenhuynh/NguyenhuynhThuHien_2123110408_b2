using Microsoft.EntityFrameworkCore;
using NguyenhuynhThuHien.Domain.Data;
using NguyenhuynhThuHien.Domain.Entity;
using NguyenhuynhThuHien_2123110408_b2.DTOs;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PatientResponse>> GetAllPatientsAsync()
        {
            return await _context.Patients
                .Select(p => new PatientResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Phone = p.Phone,
                    Email = p.Email,
                    CreatedAt = p.CreatedAt
                }).ToListAsync();
        }

        public async Task<PatientResponse?> GetPatientByIdAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return null;

            return new PatientResponse
            {
                Id = patient.Id,
                Name = patient.Name,
                Phone = patient.Phone,
                Email = patient.Email,
                CreatedAt = patient.CreatedAt
            };
        }

        public async Task<PatientResponse> CreatePatientAsync(PatientRequest request)
        {
            // Kiểm tra trùng số điện thoại
            if (await _context.Patients.AnyAsync(p => p.Phone == request.Phone))
                throw new InvalidOperationException("Số điện thoại này đã được đăng ký.");

            var newPatient = new Patient
            {
                Name = request.Name,
                Phone = request.Phone,
                Email = request.Email,
                CreatedAt = DateTime.Now
            };

            _context.Patients.Add(newPatient);
            await _context.SaveChangesAsync();

            return new PatientResponse
            {
                Id = newPatient.Id,
                Name = newPatient.Name,
                Phone = newPatient.Phone,
                Email = newPatient.Email,
                CreatedAt = newPatient.CreatedAt
            };
        }

        public async Task<bool> UpdatePatientAsync(int id, PatientRequest request)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return false;

            // Kiểm tra xem SDT mới có bị trùng với người khác (không tính chính mình) không
            if (await _context.Patients.AnyAsync(p => p.Phone == request.Phone && p.Id != id))
                throw new InvalidOperationException("Số điện thoại này đã được sử dụng bởi bệnh nhân khác.");

            patient.Name = request.Name;
            patient.Phone = request.Phone;
            patient.Email = request.Email;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return false;

            // Kiểm tra xem bệnh nhân đã có lịch khám chưa, nếu có thì không cho xóa
            var hasAppointments = await _context.Appointments.AnyAsync(a => a.PatientId == id);
            if (hasAppointments)
                throw new InvalidOperationException("Không thể xóa bệnh nhân đã có lịch khám trong hệ thống.");

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
