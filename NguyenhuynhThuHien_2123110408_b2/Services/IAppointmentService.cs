using NguyenhuynhThuHien_2123110408_b2.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public interface IAppointmentService
    {
        Task<bool> CreateAppointmentAsync(AppointmentCreateRequest request);
        Task<bool> CancelAppointmentAsync(int appointmentId);
        Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsAsync();

        // Chỉ giữ lại DUY NHẤT dòng này cho hàm tìm giờ trống
        Task<List<string>> GetAvailableTimeSlotsAsync(int dentistId, int serviceId, DateTime date);

        Task<bool> UpdateAppointmentStatusAsync(int id, byte newStatus);
        Task<IEnumerable<AppointmentResponse>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<IEnumerable<AppointmentResponse>> GetAppointmentsByDentistIdAsync(int dentistId);

        // Hàm CheckIn trả về bool (không phải trả về Object)
        Task<bool> CheckInAsync(int id);
    }
}