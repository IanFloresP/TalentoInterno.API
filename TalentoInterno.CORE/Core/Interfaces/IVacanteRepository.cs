using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IVacanteRepository
{
    Task<IEnumerable<Vacante>> GetAllAsync();
    Task<Vacante?> GetByIdAsync(int id);
    Task AddAsync(Vacante vacante);
    Task UpdateAsync(Vacante vacante);
    Task DeleteAsync(int id);
}