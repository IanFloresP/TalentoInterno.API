using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IPerfilService
{
    Task<IEnumerable<Perfil>> GetAllPerfilesAsync();
    Task<Perfil?> GetPerfilByIdAsync(int id);
    Task CreatePerfilAsync(Perfil perfil);
    Task UpdatePerfilAsync(Perfil perfil);
    Task DeletePerfilAsync(int id);
}