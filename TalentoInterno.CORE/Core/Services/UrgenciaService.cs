using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class UrgenciaService : IUrgenciaService
{
    private readonly IUrgenciaRepository _repository;

    public UrgenciaService(IUrgenciaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Urgencia>> GetAllUrgenciasAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Urgencia?> GetUrgenciaByIdAsync(byte id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateUrgenciaAsync(Urgencia urgencia)
    {
        await _repository.AddAsync(urgencia);
    }

    public async Task UpdateUrgenciaAsync(Urgencia urgencia)
    {
        await _repository.UpdateAsync(urgencia);
    }

    public async Task DeleteUrgenciaAsync(byte id)
    {
        await _repository.DeleteAsync(id);
    }
}