using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IAreaRepository
    {
        Task AddAsync(Area area);
        Task DeleteAsync(int id);
        Task<IEnumerable<Area>> GetAllAsync();
        Task<Area?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task UpdateAsync(Area area);
    }
}