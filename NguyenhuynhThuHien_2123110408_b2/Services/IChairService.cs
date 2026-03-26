using NguyenhuynhThuHien_2123110408_b2.DTOs;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public interface IChairService
    {
        Task<IEnumerable<ChairResponse>> GetAllChairsAsync();
        Task<ChairResponse?> GetChairByIdAsync(int id);
        Task<ChairResponse> CreateChairAsync(ChairRequest request);
        Task<bool> UpdateChairAsync(int id, ChairRequest request);
        Task<bool> DeleteChairAsync(int id);
    }
}
