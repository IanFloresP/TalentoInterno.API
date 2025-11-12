using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class PerfilService : IPerfilService
{
    private readonly IPerfilRepository _repository;

    public PerfilService(IPerfilRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Perfil>> GetAllPerfilesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Perfil?> GetPerfilByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreatePerfilAsync(Perfil perfil)
    {
        await _repository.AddAsync(perfil);
    }

    public async Task UpdatePerfilAsync(Perfil perfil)
    {
        await _repository.UpdateAsync(perfil);
    }

    public async Task DeletePerfilAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}