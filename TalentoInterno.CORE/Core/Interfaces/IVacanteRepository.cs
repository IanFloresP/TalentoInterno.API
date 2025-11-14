using TalentoInterno.CORE.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IVacanteRepository
{
    // Métodos de Lectura
    Task<IEnumerable<Vacante>> GetAllAsync();
    Task<Vacante?> GetByIdAsync(int id);

    // Métodos de Escritura (CRUD)
    Task AddAsync(Vacante vacante);
    Task UpdateAsync(Vacante vacante);
    Task DeleteAsync(int id);

    // Método de Unidad de Trabajo (Utilizado en la transacción de VacanteService)
    Task SaveChangesAsync();
}