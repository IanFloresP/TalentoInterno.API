using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class ColaboradorService : IColaboradorService
{
    private readonly IColaboradorRepository _repository;

    public ColaboradorService(IColaboradorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Colaborador>> GetAllColaboradoresAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Colaborador?> GetColaboradorByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateColaboradorAsync(Colaborador colaborador)
    {
        await _repository.AddAsync(colaborador);
    }

    public async Task UpdateColaboradorAsync(Colaborador colaborador)
    {
        await _repository.UpdateAsync(colaborador);
    }

    public async Task DeleteColaboradorAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}