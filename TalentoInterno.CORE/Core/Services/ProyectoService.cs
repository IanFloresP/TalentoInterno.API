using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class ProyectoService : IProyectoService
{
    private readonly IProyectoRepository _repository;

    public ProyectoService(IProyectoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Proyecto>> GetAllProyectosAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Proyecto?> GetProyectoByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateProyectoAsync(Proyecto proyecto)
    {
        await _repository.AddAsync(proyecto);
    }

    public async Task UpdateProyectoAsync(Proyecto proyecto)
    {
        await _repository.UpdateAsync(proyecto);
    }

    public async Task DeleteProyectoAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}