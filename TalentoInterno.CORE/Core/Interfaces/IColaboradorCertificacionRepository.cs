using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;
public interface IColaboradorCertificacionRepository
{
    Task AddAsync(ColaboradorCertificacion certificacion);
    Task DeleteAsync(global::System.Int32 colaboradorId, global::System.Int32 certificacionId);
    Task<IEnumerable<ColaboradorCertificacion>> GetByColaboradorIdAsync(global::System.Int32 colaboradorId);
    Task SaveChangesAsync();
}