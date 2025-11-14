using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IRolRepository
    {
        Task AddAsync(Rol rol);
        Task DeleteAsync(int id);
        Task<IEnumerable<Rol>> GetAllAsync();
        Task<Rol?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task UpdateAsync(Rol rol);
    }
}