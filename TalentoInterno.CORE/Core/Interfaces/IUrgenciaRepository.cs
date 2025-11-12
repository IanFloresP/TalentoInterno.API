using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IUrgenciaRepository
{
    Task<IEnumerable<Urgencia>> GetAllAsync();
    Task<Urgencia?> GetByIdAsync(byte id);
    Task AddAsync(Urgencia urgencia);
    Task UpdateAsync(Urgencia urgencia);
    Task DeleteAsync(byte id);
}