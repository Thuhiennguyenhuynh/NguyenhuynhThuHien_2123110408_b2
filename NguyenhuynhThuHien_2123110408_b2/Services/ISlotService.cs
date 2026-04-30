using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NguyenhuynhThuHien_2123110408_b2.Services
{
    public interface ISlotService
    {
        Task<IEnumerable<string>> GetAvailableSlotsAsync(DateTime date, int dentistId, int serviceId);
    }
}
