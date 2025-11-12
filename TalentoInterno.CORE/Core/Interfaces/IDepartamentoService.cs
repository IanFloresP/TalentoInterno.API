using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IDepartamentoService
{
    Task<IEnumerable<Departamento>> GetAllDepartamentosAsync();
    Task<Departamento?> GetDepartamentoByIdAsync(int id);
    Task CreateDepartamentoAsync(Departamento departamento);
    Task UpdateDepartamentoAsync(Departamento departamento);
    Task DeleteDepartamentoAsync(int id);
}
