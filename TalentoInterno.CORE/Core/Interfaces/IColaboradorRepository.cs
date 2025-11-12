using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IColaboradorRepository
    {
        Task AddAsync(Colaborador colaborador);
        Task DeleteAsync(int id);
        Task<IEnumerable<Colaborador>> GetAllAsync();
        Task<Colaborador?> GetByIdAsync(int id);
        Task UpdateAsync(Colaborador colaborador);
    }
}