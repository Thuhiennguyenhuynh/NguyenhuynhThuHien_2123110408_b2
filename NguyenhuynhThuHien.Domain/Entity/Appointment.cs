using System;
using NguyenhuynhThuHien.Domain.Entity.NguyenhuynhThuHien_2123110408_b2.Models;

namespace NguyenhuynhThuHien.Domain.Entity
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DentistId { get; set; }
        public int ServiceId { get; set; }
        public int? ChairId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public byte Status { get; set; }

        // Thêm dấu ? để fix Warning CS8618
        public string? Note { get; set; }
        public string? BookingSource { get; set; }

        // ==========================================
        // THÊM ĐOẠN NÀY ĐỂ FIX 3 LỖI CS1061
        // ==========================================
        public virtual Patient Patient { get; set; }
        public virtual Dentist Dentist { get; set; }
        public virtual Service Service { get; set; }
        public virtual Chair Chair { get; set; }
    }
}