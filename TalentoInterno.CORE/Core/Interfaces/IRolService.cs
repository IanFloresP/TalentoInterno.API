using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IRolService
{
    Task<IEnumerable<Rol>> GetAllRolesAsync();
    Task<Rol?> GetRoleByIdAsync(int id);
    Task CreateRoleAsync(Rol rol);
    Task UpdateRoleAsync(Rol rol);
    Task DeleteRoleAsync(int id);
}
