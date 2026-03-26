using NguyenhuynhThuHien_2123110408_b2.DTOs;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public interface IDentalService
    {
        Task<IEnumerable<ServiceResponse>> GetAllServicesAsync();
        Task<ServiceResponse?> GetServiceByIdAsync(int id);
        Task<ServiceResponse> CreateServiceAsync(ServiceRequest request);
        Task<bool> UpdateServiceAsync(int id, ServiceRequest request);
        Task<bool> DeleteServiceAsync(int id);
    }
}
