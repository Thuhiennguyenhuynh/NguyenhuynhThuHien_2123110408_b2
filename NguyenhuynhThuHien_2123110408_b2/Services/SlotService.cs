using Microsoft.EntityFrameworkCore;
using NguyenhuynhThuHien.Domain.Data;
using NguyenhuynhThuHien.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public class SlotService : ISlotService
    {
        private readonly ApplicationDbContext _context;

        public SlotService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetAvailableSlotsAsync(DateTime date, int dentistId, int serviceId)
        {
            // 1. Lấy thông tin Dịch vụ để biết Duration
            var service = await _context.Set<Service>().FindAsync(serviceId);
            if (service == null) return new List<string>();

            int durationMinutes = service.Duration;

            // 2. Lấy các lịch hẹn của bác sĩ trong ngày được chọn
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            var bookedAppointments = await _context.Set<Appointment>()
                .Where(a => a.DentistId == dentistId
                         && a.StartTime >= startOfDay
                         && a.StartTime < endOfDay
                         && a.Status != 5) // Bỏ qua các lịch đã Hủy (Cancelled = 5)
                .ToListAsync();

            // 3. Khởi tạo khung giờ làm việc: 08:00 - 17:00
            var availableSlots = new List<string>();
            var workStart = startOfDay.AddHours(8);  // 08:00
            var workEnd = startOfDay.AddHours(17);   // 17:00
            var step = TimeSpan.FromMinutes(30);     // Bước nhảy 30 phút

            var currentSlot = workStart;

            // 4. Duyệt qua từng khung giờ
            while (currentSlot.AddMinutes(durationMinutes) <= workEnd)
            {
                var potentialEnd = currentSlot.AddMinutes(durationMinutes);

                // Kiểm tra điều kiện Overlap: 
                // Thời gian bắt đầu của slot mới phải nhỏ hơn EndTime của lịch cũ 
                // VÀ thời gian kết thúc của slot mới phải lớn hơn StartTime của lịch cũ
                bool isOverlap = bookedAppointments.Any(a =>
                    currentSlot < a.EndTime && potentialEnd > a.StartTime
                );

                if (!isOverlap)
                {
                    availableSlots.Add(currentSlot.ToString("HH:mm"));
                }

                currentSlot = currentSlot.Add(step);
            }

            return availableSlots;
        }
    }
}