using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IRolService
    {
        Task CreateRoleAsync(Rol rol);
        Task DeleteRoleAsync(int id);
        Task<IEnumerable<Rol>> GetAllRolesAsync();
        Task<Rol?> GetRoleByIdAsync(int id);
        Task UpdateRoleAsync(Rol rol);
    }
}