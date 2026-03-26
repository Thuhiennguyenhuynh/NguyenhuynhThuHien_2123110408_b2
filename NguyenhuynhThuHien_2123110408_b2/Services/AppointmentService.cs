using Microsoft.EntityFrameworkCore;
using NguyenhuynhThuHien.Domain.Data;
using NguyenhuynhThuHien_2123110408_b2.DTOs;

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
        // 1. TÍNH NĂNG ĐẶT LỊCH (Đã làm xong)
        // ==========================================
        public async Task<bool> CreateAppointmentAsync(AppointmentCreateRequest request)
        {
            // Rule 6.2: Không đặt lịch trong quá khứ
            if (request.StartTime <= DateTime.Now)
            {
                throw new ArgumentException("Thời gian đặt lịch không hợp lệ (phải lớn hơn hiện tại).");
            }

            var service = await _context.Services.FindAsync(request.ServiceId);
            if (service == null)
            {
                throw new ArgumentException("Dịch vụ không tồn tại.");
            }

            // Rule 6.3: Tự động tính EndTime = StartTime + Duration
            DateTime endTime = request.StartTime.AddMinutes(service.Duration);
            byte status = 0; // 0: Pending

            try
            {
                // Gọi Stored Procedure đã tạo ở Database
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC CreateAppointment {request.PatientId}, {request.DentistId}, {request.ChairId}, {request.ServiceId}, {request.StartTime}, {endTime}, {status}"
                );
                return true;
            }
            catch (Exception ex)
            {
                // Bắt lỗi từ lệnh THROW 50001 trong Stored Procedure
                if (ex.Message.Contains("Trùng lịch"))
                {
                    throw new InvalidOperationException(ex.Message);
                }
                throw;
            }
        }

        // ==========================================
        // 2. TÍNH NĂNG HỦY LỊCH (Mới thêm vào)
        // ==========================================
        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            // Tìm lịch khám trong DB
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                throw new ArgumentException("Không tìm thấy lịch khám này.");
            }

            // Rule 6.4: Không cho hủy lịch trước 2 tiếng so với giờ bắt đầu
            var timeDifference = appointment.StartTime - DateTime.Now;
            if (timeDifference.TotalHours < 2)
            {
                throw new InvalidOperationException("Không thể hủy lịch! Bạn chỉ được phép hủy trước ít nhất 2 giờ so với thời gian khám.");
            }

            // Nếu hợp lệ, chuyển Status thành 5 (Cancelled)
            appointment.Status = 5;

            // Lưu xuống DB
            await _context.SaveChangesAsync();
            return true;
        }

        // ==========================================
        // 3. TÍNH NĂNG LẤY DANH SÁCH (GET)
        // ==========================================
        public async Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsAsync()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Include(a => a.Chair)
                .Include(a => a.Service)
                .Select(a => new AppointmentResponse
                {
                    Id = a.Id,
                    PatientName = a.Patient.Name,
                    DentistName = a.Dentist.Name,
                    ChairName = a.Chair.Name,
                    ServiceName = a.Service.Name,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    // Gọi hàm phụ để dịch Status dạng số sang dạng chữ theo SRS mục 3
                    StatusText = GetStatusText(a.Status)
                })
                .ToListAsync();

            return appointments;
        }

        // Hàm phụ (Helper method) giúp dịch số thành chữ cho người dùng dễ đọc
        private static string GetStatusText(byte status)
        {
            return status switch
            {
                0 => "Pending",
                1 => "Confirmed",
                2 => "CheckedIn",
                3 => "InTreatment",
                4 => "Completed",
                5 => "Cancelled",
                6 => "NoShow",
                _ => "Unknown"
            };
        }

        // ==========================================
        // 4. TÍNH NĂNG TÌM GIỜ TRỐNG (GET TIMESLOTS)
        // ==========================================
        public async Task<List<string>> GetAvailableTimeSlotsAsync(int dentistId, DateTime date)
        {
            // 1. Lấy danh sách các lịch ĐÃ ĐẶT của Nha sĩ này trong ngày hôm đó
            // Bỏ qua các lịch đã bị hủy (Status = 5)
            var busyAppointments = await _context.Appointments
                .Where(a => a.DentistId == dentistId
                         && a.StartTime.Date == date.Date
                         && a.Status != 5)
                .ToListAsync();

            // 2. Tạo khung giờ làm việc chuẩn: Từ 8h00 sáng đến 17h00 chiều
            // Giả sử mỗi ca khám chia làm các block 30 phút
            var allWorkingSlots = new List<TimeSpan>();
            for (int hour = 8; hour < 17; hour++)
            {
                // Nghỉ trưa từ 12h - 13h
                if (hour == 12) continue;

                allWorkingSlots.Add(new TimeSpan(hour, 0, 0));  // Chẵn giờ (VD: 08:00)
                allWorkingSlots.Add(new TimeSpan(hour, 30, 0)); // Rưỡi (VD: 08:30)
            }

            // 3. Lọc ra những khung giờ CHƯA BỊ TRÙNG
            var availableSlots = new List<string>();

            foreach (var slot in allWorkingSlots)
            {
                var slotStartTime = date.Date.Add(slot);
                var slotEndTime = slotStartTime.AddMinutes(30); // Giả định check block 30p

                // Kiểm tra xem slot này có bị đè vào bất kỳ Appointment nào đã tồn tại không
                bool isBusy = busyAppointments.Any(a =>
                    (slotStartTime >= a.StartTime && slotStartTime < a.EndTime) || // Bắt đầu giữa chừng ca khác
                    (slotEndTime > a.StartTime && slotEndTime <= a.EndTime) ||     // Kết thúc giữa chừng ca khác
                    (slotStartTime <= a.StartTime && slotEndTime >= a.EndTime)     // Trùm ra ngoài ca khác
                );

                // Nếu thời gian check là ở quá khứ so với hiện tại (ví dụ tìm giờ cho ngày hôm nay) thì bỏ qua
                if (slotStartTime <= DateTime.Now)
                {
                    isBusy = true;
                }

                // Nếu không bận, thêm vào danh sách kết quả trả về
                if (!isBusy)
                {
                    availableSlots.Add(slot.ToString(@"hh\:mm")); // Trả về dạng "08:00", "08:30"
                }
            }

            return availableSlots;
        }


        // ==========================================
        // 5. TÍNH NĂNG CẬP NHẬT TRẠNG THÁI (PUT)
        // ==========================================
        public async Task<bool> UpdateAppointmentStatusAsync(int id, byte newStatus)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                throw new ArgumentException("Không tìm thấy lịch khám.");
            }

            // Có thể thêm logic rule ở đây (VD: Không cho chuyển từ Completed ngược về Pending)
            if (appointment.Status == 4 && newStatus < 4)
            {
                throw new InvalidOperationException("Lịch khám đã hoàn thành, không thể quay ngược trạng thái.");
            }

            appointment.Status = newStatus;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}