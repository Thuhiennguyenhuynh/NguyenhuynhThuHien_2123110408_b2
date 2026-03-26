using NguyenhuynhThuHien_2123110408_b2.DTOs;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientResponse>> GetAllPatientsAsync();
        Task<PatientResponse?> GetPatientByIdAsync(int id);
        Task<PatientResponse> CreatePatientAsync(PatientRequest request);
        Task<bool> UpdatePatientAsync(int id, PatientRequest request);
        Task<bool> DeletePatientAsync(int id);
    }
}
