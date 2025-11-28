using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IPostulacionRepository
    {
        Task AddAsync(Postulacion postulacion);
        Task<IEnumerable<Postulacion>> GetByColaboradorIdAsync(int colaboradorId);
        Task<Postulacion?> GetByIdAsync(int id);
        Task<IEnumerable<Postulacion>> GetByVacanteIdAsync(int vacanteId);
        Task SaveChangesAsync();
        Task UpdateAsync(Postulacion postulacion);
    }
}