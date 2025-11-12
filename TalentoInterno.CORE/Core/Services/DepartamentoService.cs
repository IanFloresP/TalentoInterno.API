using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class DepartamentoService : IDepartamentoService
{
    private readonly IDepartamentoRepository _repository;

    public DepartamentoService(IDepartamentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Departamento>> GetAllDepartamentosAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Departamento?> GetDepartamentoByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateDepartamentoAsync(Departamento departamento)
    {
        await _repository.AddAsync(departamento);
    }

    public async Task UpdateDepartamentoAsync(Departamento departamento)
    {
        await _repository.UpdateAsync(departamento);
    }

    public async Task DeleteDepartamentoAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
