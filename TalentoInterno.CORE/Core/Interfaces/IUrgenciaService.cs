using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IUrgenciaService
{
    Task<IEnumerable<Urgencia>> GetAllUrgenciasAsync();
    Task<Urgencia?> GetUrgenciaByIdAsync(byte id);
    Task CreateUrgenciaAsync(Urgencia urgencia);
    Task UpdateUrgenciaAsync(Urgencia urgencia);
    Task DeleteUrgenciaAsync(byte id);
}