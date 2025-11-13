using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IRolRepository
{
    Task<IEnumerable<Rol>> GetAllAsync();
    Task<Rol?> GetByIdAsync(int id);
    Task AddAsync(Rol rol);
    Task UpdateAsync(Rol rol);
    Task DeleteAsync(int id);
}
