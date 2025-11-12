using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface ICuentaService
{
    Task<IEnumerable<Cuenta>> GetAllCuentasAsync();
    Task<Cuenta?> GetCuentaByIdAsync(int id);
    Task CreateCuentaAsync(Cuenta cuenta);
    Task UpdateCuentaAsync(Cuenta cuenta);
    Task DeleteCuentaAsync(int id);
}