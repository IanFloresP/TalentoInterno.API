using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class CuentaService : ICuentaService
{
    private readonly ICuentaRepository _repository;

    public CuentaService(ICuentaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Cuenta>> GetAllCuentasAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Cuenta?> GetCuentaByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateCuentaAsync(Cuenta cuenta)
    {
        await _repository.AddAsync(cuenta);
    }

    public async Task UpdateCuentaAsync(Cuenta cuenta)
    {
        await _repository.UpdateAsync(cuenta);
    }

    public async Task DeleteCuentaAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}