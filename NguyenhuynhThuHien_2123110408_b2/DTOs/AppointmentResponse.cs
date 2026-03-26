namespace NguyenhuynhThuHien_2123110408_b2.DTOs
{
    public class AppointmentResponse
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DentistName { get; set; } = string.Empty;
        public string ChairName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string StatusText { get; set; } = string.Empty;
    }
}
