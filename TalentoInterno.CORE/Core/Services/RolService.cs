using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class RolService : IRolService
{
    private readonly IRolRepository _repository;

    public RolService(IRolRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Rol>> GetAllRolesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Rol?> GetRoleByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateRoleAsync(Rol rol)
    {
        await _repository.AddAsync(rol);
    }

    public async Task UpdateRoleAsync(Rol rol)
    {
        await _repository.UpdateAsync(rol);
    }

    public async Task DeleteRoleAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
