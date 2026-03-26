using NguyenhuynhThuHien_2123110408_b2.DTOs;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public interface IDentistService
    {
        Task<IEnumerable<DentistResponse>> GetAllDentistsAsync();
        Task<DentistResponse?> GetDentistByIdAsync(int id);
        Task<DentistResponse> CreateDentistAsync(DentistRequest request);
        Task<bool> UpdateDentistAsync(int id, DentistRequest request);
        Task<bool> DeleteDentistAsync(int id);
    }   
}
