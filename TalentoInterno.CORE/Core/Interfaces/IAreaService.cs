using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IAreaService
    {
        Task<AreaDto> CreateAreaAsync(AreaDto areaDto);
        Task DeleteAreaAsync(int id);
        Task<IEnumerable<AreaDto>> GetAllAreasAsync();
        Task<AreaDto?> GetAreaByIdAsync(int id);
        Task UpdateAreaAsync(int id, AreaDto areaDto);
    }
}