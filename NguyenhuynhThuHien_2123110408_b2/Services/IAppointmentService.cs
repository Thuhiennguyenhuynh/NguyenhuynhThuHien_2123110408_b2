using NguyenhuynhThuHien_2123110408_b2.DTOs;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public interface IAppointmentService
    {
        Task<bool> CreateAppointmentAsync(AppointmentCreateRequest request);
        Task<bool> CancelAppointmentAsync(int appointmentId);

        // lấy danh sách toàn bộ
        Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsAsync();

        // Tìm các khung giờ còn trống của 1 nha sĩ trong 1 ngày cụ thể
        Task<List<string>> GetAvailableTimeSlotsAsync(int dentistId, DateTime date);

        Task<bool> UpdateAppointmentStatusAsync(int id, byte newStatus);

        // Lấy lịch hẹn theo Bệnh nhân (Đã xóa dòng thừa)
        Task<IEnumerable<AppointmentResponse>> GetAppointmentsByPatientIdAsync(int patientId);
    }
}