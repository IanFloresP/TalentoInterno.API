using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IVacanteRepository
    {
        Task AddAsync(Vacante vacante);
        Task DeleteAsync(int id);
        Task<IEnumerable<Vacante>> GetAllAsync();
        Task<Vacante?> GetByIdAsync(int id);
        Task UpdateAsync(Vacante vacante);
    }
}