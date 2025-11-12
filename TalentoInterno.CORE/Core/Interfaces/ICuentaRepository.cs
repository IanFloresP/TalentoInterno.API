using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface ICuentaRepository
{
    Task<IEnumerable<Cuenta>> GetAllAsync();
    Task<Cuenta?> GetByIdAsync(int id);
    Task AddAsync(Cuenta cuenta);
    Task UpdateAsync(Cuenta cuenta);
    Task DeleteAsync(int id);
}