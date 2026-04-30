using NguyenhuynhThuHien.Domain.Entity;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public interface IReceptionistService
    {
        Task<IEnumerable<Receptionist>> GetAllAsync();
        Task<Receptionist> GetByIdAsync(int id);
        Task<Receptionist> CreateAsync(Receptionist receptionist);
        Task<Receptionist> UpdateAsync(int id, Receptionist receptionist);
        Task<bool> DeleteAsync(int id);
    }
}
