using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class NivelDominioService : INivelDominioService
{
    private readonly INivelDominioRepository _repository;

    public NivelDominioService(INivelDominioRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<NivelDominio>> GetAllNivelesDominioAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<NivelDominio?> GetNivelDominioByIdAsync(byte id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateNivelDominioAsync(NivelDominio nivelDominio)
    {
        await _repository.AddAsync(nivelDominio);
    }

    public async Task UpdateNivelDominioAsync(NivelDominio nivelDominio)
    {
        await _repository.UpdateAsync(nivelDominio);
    }

    public async Task DeleteNivelDominioAsync(byte id)
    {
        await _repository.DeleteAsync(id);
    }
}