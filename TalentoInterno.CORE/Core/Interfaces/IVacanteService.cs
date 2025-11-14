using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IVacanteService
{
    Task<IEnumerable<Vacante>> GetAllVacantesAsync();
    Task<Vacante?> GetVacanteByIdAsync(int id);
    Task CreateVacanteAsync(Vacante vacante);
    Task UpdateVacanteAsync(Vacante vacante);
    Task DeleteVacanteAsync(int id);
}