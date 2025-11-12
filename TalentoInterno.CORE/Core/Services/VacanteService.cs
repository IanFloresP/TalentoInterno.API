using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class VacanteService : IVacanteService
{
    private readonly IVacanteRepository _repository;

    public VacanteService(IVacanteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Vacante>> GetAllVacantesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Vacante?> GetVacanteByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateVacanteAsync(Vacante vacante)
    {
        await _repository.AddAsync(vacante);
    }

    public async Task UpdateVacanteAsync(Vacante vacante)
    {
        await _repository.UpdateAsync(vacante);
    }

    public async Task DeleteVacanteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}