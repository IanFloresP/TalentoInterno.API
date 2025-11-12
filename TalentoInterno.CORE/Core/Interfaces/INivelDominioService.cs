using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface INivelDominioService
{
    Task<IEnumerable<NivelDominio>> GetAllNivelesDominioAsync();
    Task<NivelDominio?> GetNivelDominioByIdAsync(byte id);
    Task CreateNivelDominioAsync(NivelDominio nivelDominio);
    Task UpdateNivelDominioAsync(NivelDominio nivelDominio);
    Task DeleteNivelDominioAsync(byte id);
}