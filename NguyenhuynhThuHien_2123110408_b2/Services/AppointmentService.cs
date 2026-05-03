using Microsoft.EntityFrameworkCore;
using NguyenhuynhThuHien.Domain.Data;
using NguyenhuynhThuHien.Domain.Entity;
using NguyenhuynhThuHien_2123110408_b2.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. TÍNH NĂNG ĐẶT LỊCH (Update theo BA: Không gán ghế)
        // ==========================================
        public async Task<bool> CreateAppointmentAsync(AppointmentCreateRequest request)
        {
            // Rule: Không đặt lịch trong quá khứ
            if (request.StartTime <= DateTime.Now)
            {
                throw new ArgumentException("Thời gian đặt lịch không hợp lệ (phải lớn hơn hiện tại).");
            }

            var service = await _context.Services.FindAsync(request.ServiceId);
            if (service == null)
            {
                throw new ArgumentException("Dịch vụ không tồn tại.");
            }

            // Rule BR-05: Tự động tính EndTime = StartTime + Duration
            DateTime endTime = request.StartTime.AddMinutes(service.Duration);

            // Rule BR-03: Không cho phép đặt lịch ngoài giờ làm việc (08:00–17:00)
            var workStart = request.StartTime.Date.AddHours(8);
            var workEnd = request.StartTime.Date.AddHours(17);
            if (request.StartTime < workStart || endTime > workEnd)
            {
                throw new ArgumentException("Không cho phép đặt lịch ngoài giờ làm việc (08:00 – 17:00).");
            }

            // Rule BR-01: Kiểm tra trùng lịch bác sĩ (overlap check)
            bool isDentistBooked = await _context.Appointments.AnyAsync(a =>
                a.DentistId == request.DentistId &&
                a.Status <= 2 && // Pending, Confirmed, CheckedIn
                (request.StartTime < a.EndTime && endTime > a.StartTime));

            if (isDentistBooked)
            {
                throw new InvalidOperationException("Bác sĩ đã có lịch trong khung giờ này.");
            }

            // Tạo lịch hẹn mới
            var newAppointment = new Appointment
            {
                PatientId = request.PatientId,
                DentistId = request.DentistId,
                ServiceId = request.ServiceId,
                StartTime = request.StartTime,
                EndTime = endTime,
                Status = 0, // 0: Pending
                ChairId = null, // BA Update: KHÔNG gán ghế lúc đặt lịch, chỉ gán khi Check-in
                BookingSource = "Online"
            };

            _context.Appointments.Add(newAppointment);
            await _context.SaveChangesAsync();
            return true;
        }

        // ==========================================
        // 2. TÍNH NĂNG CHECK-IN VÀ GÁN GHẾ (Mới bổ sung theo SRS)
        // ==========================================
        public async Task<bool> CheckInAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null) throw new ArgumentException("Không tìm thấy lịch khám.");

            // Chỉ cho phép Check-in khi lịch đã Confirm (1)
            if (appointment.Status != 1)
                throw new InvalidOperationException("Chỉ có thể Check-in cho lịch hẹn đã được Xác nhận (Confirmed).");

            // Lấy danh sách ghế đang rảnh (Status = 1)[cite: 4]
            var availableChairs = await _context.Chairs.Where(c => c.Status == 1).ToListAsync();

            int? selectedChairId = null;
            foreach (var chair in availableChairs)
            {
                // Rule BR-02: Kiểm tra ghế có bị trùng lịch tại thời điểm khám không[cite: 4]
                bool isChairOccupied = await _context.Appointments.AnyAsync(a =>
                    a.ChairId == chair.Id &&
                    (a.Status == 2 || a.Status == 3) && // Đang CheckedIn hoặc InProgress
                    (appointment.StartTime < a.EndTime && appointment.EndTime > a.StartTime));

                if (!isChairOccupied)
                {
                    selectedChairId = chair.Id;
                    break;
                }
            }

            if (selectedChairId == null)
                throw new InvalidOperationException("Hiện tại phòng khám đã hết ghế trống. Vui lòng chờ.");

            // Gán ghế và cập nhật trạng thái
            appointment.ChairId = selectedChairId;
            appointment.Status = 2; // 2: CheckedIn[cite: 4]

            await _context.SaveChangesAsync();
            return true;
        }

        // ==========================================
        // 3. TÍNH NĂNG HỦY LỊCH
        // ==========================================
        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null) throw new ArgumentException("Không tìm thấy lịch khám này.");

            // Chỉ cho phép hủy khi trạng thái là Pending (0) hoặc Confirmed (1)
            if (appointment.Status != 0 && appointment.Status != 1)
            {
                throw new InvalidOperationException("Chỉ có thể hủy lịch hẹn ở trạng thái Pending hoặc Confirmed.");
            }

            // Rule BR-04: Không cho hủy lịch trước 2 tiếng
            var timeDifference = appointment.StartTime - DateTime.Now;
            if (timeDifference.TotalHours < 2)
            {
                throw new InvalidOperationException("Không thể hủy lịch! Bạn chỉ được phép hủy trước ít nhất 2 giờ so với thời gian khám.");
            }

            appointment.Status = 5; // Cancelled
            await _context.SaveChangesAsync();
            return true;
        }

        // ==========================================
        // 4. TÍNH NĂNG TÌM GIỜ TRỐNG (Cập nhật logic Duration)
        // ==========================================
        public async Task<List<string>> GetAvailableTimeSlotsAsync(int dentistId, int serviceId, DateTime date)
        {
            // Lấy Duration của dịch vụ[cite: 4]
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null) throw new ArgumentException("Dịch vụ không tồn tại.");
            int durationMinutes = service.Duration;

            // Lấy lịch của nha sĩ (bỏ lịch Hủy và NoShow)[cite: 4]
            var busyAppointments = await _context.Appointments
                .Where(a => a.DentistId == dentistId
                         && a.StartTime.Date == date.Date
                         && a.Status != 5 && a.Status != 6)
                .ToListAsync();

            var availableSlots = new List<string>();
            var workStart = date.Date.AddHours(8);  // 08:00[cite: 4]
            var workEnd = date.Date.AddHours(17);   // 17:00[cite: 4]
            var step = TimeSpan.FromMinutes(30);

            var currentSlot = workStart;

            while (currentSlot.AddMinutes(durationMinutes) <= workEnd)
            {

                var potentialEnd = currentSlot.AddMinutes(durationMinutes);

                // Kiểm tra Overlap với lịch khác[cite: 4]
                bool isBusy = busyAppointments.Any(a => currentSlot < a.EndTime && potentialEnd > a.StartTime);

                if (currentSlot <= DateTime.Now)
                {
                    isBusy = true;
                }

                if (!isBusy)
                {
                    availableSlots.Add(currentSlot.ToString("HH:mm"));
                }

                currentSlot = currentSlot.Add(step);
            }

            return availableSlots;
        }

        // ==========================================
        // 5. CÁC HÀM GET (Lấy dữ liệu)
        // ==========================================
        public async Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Include(a => a.Chair)
                .Include(a => a.Service)
                .Select(a => new AppointmentResponse
                {
                    Id = a.Id,
                    PatientName = a.Patient.Name,
                    DentistName = a.Dentist.Name,
                    // Xử lý an toàn vì ChairId giờ có thể null[cite: 4]
                    ChairName = a.Chair != null ? a.Chair.Name : "Chưa gán ghế",
                    ServiceName = a.Service.Name,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    StatusText = GetStatusText(a.Status)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentResponse>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Include(a => a.Chair)
                .Include(a => a.Service)
                .Where(a => a.PatientId == patientId)
                .Select(a => new AppointmentResponse
                {
                    Id = a.Id,
                    PatientName = a.Patient.Name,
                    DentistName = a.Dentist.Name,
                    ChairName = a.Chair != null ? a.Chair.Name : "Chưa gán ghế",
                    ServiceName = a.Service.Name,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    StatusText = GetStatusText(a.Status)
                })
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentResponse>> GetAppointmentsByDentistIdAsync(int dentistId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Chair)
                .Where(a => a.DentistId == dentistId && a.Status != 5)
                .OrderBy(a => a.StartTime)
                .Select(a => new AppointmentResponse
                {
                    Id = a.Id,
                    PatientName = a.Patient.Name,
                    ServiceName = a.Service.Name,
                    ChairName = a.Chair != null ? a.Chair.Name : "Chưa gán ghế",
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    StatusText = GetStatusText(a.Status)
                })
                .ToListAsync();
        }

        // ==========================================
        // 6. CẬP NHẬT TRẠNG THÁI CHUNG
        // ==========================================
        public async Task<bool> UpdateAppointmentStatusAsync(int id, byte newStatus)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) throw new ArgumentException("Không tìm thấy lịch khám.");

            // Validate luồng trạng thái theo SRS:
            // Pending(0) → Confirmed(1) → CheckedIn(2) → InProgress(3) → Completed(4)
            // Pending(0)/Confirmed(1) → Cancelled(5)
            bool isValidTransition = (appointment.Status, newStatus) switch
            {
                (0, 1) => true,  // Pending → Confirmed
                (0, 5) => true,  // Pending → Cancelled
                (1, 2) => true,  // Confirmed → CheckedIn (qua CheckInAsync)
                (1, 5) => true,  // Confirmed → Cancelled
                (2, 3) => true,  // CheckedIn → InProgress
                (3, 4) => true,  // InProgress → Completed
                _ => false
            };

            if (!isValidTransition)
            {
                throw new InvalidOperationException(
                    $"Không thể chuyển trạng thái từ {GetStatusText(appointment.Status)} sang {GetStatusText(newStatus)}.");
            }

            appointment.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        // Hàm phụ
        private static string GetStatusText(byte status)
        {
            return status switch
            {
                0 => "Pending",
                1 => "Confirmed",
                2 => "CheckedIn",
                3 => "InProgress",
                4 => "Completed",
                5 => "Cancelled",
                6 => "NoShow",
                _ => "Unknown"
            };
        }
        public async Task<AppointmentResponse> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Include(a => a.Chair)
                .Include(a => a.Service)
                .Where(a => a.Id == id)
                .Select(a => new AppointmentResponse
                {
                    Id = a.Id,
                    PatientName = a.Patient.Name,
                    DentistName = a.Dentist.Name,
                    ChairName = a.Chair != null ? a.Chair.Name : "Chưa gán ghế",
                    ServiceName = a.Service.Name,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    StatusText = GetStatusText(a.Status)
                    // LƯU Ý: Nếu trong AppointmentResponse của bạn (file DTO) có khai báo thêm 
                    // các trường như PatientPhone, Price, Note... thì bạn bổ sung map vào đây nhé!
                })
                .FirstOrDefaultAsync();

            return appointment; // Sẽ trả về null nếu không tìm thấy
        }
    }
}