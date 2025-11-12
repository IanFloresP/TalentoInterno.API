using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IProyectoService
{
    Task<IEnumerable<Proyecto>> GetAllProyectosAsync();
    Task<Proyecto?> GetProyectoByIdAsync(int id);
    Task CreateProyectoAsync(Proyecto proyecto);
    Task UpdateProyectoAsync(Proyecto proyecto);
    Task DeleteProyectoAsync(int id);
}