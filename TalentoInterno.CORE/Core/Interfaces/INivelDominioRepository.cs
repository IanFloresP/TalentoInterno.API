using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface INivelDominioRepository
{
    Task<IEnumerable<NivelDominio>> GetAllAsync();
    Task<NivelDominio?> GetByIdAsync(byte id);
    Task AddAsync(NivelDominio nivelDominio);
    Task UpdateAsync(NivelDominio nivelDominio);
    Task DeleteAsync(byte id);
}