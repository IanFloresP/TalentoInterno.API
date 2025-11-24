using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IAuditoriaRepository
    {
        Task AddAsync(Auditoria auditoria);
        Task<IEnumerable<Auditoria>> GetFiltradoAsync(int? usuarioId, DateTime? desde, DateTime? hasta, string? accion);
        Task SaveChangesAsync();
    }
}