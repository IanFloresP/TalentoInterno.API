using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface ICertificacionRepository
    {
        Task AddAsync(Certificacion certificacion);
        Task DeleteAsync(int id);
        Task<IEnumerable<Certificacion>> GetAllAsync();
        Task<Certificacion?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task UpdateAsync(Certificacion certificacion);
    }
}