using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;

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
        // Aquí podrías añadir lógica de negocio, como:
        // if (await _repository.GetByNameAsync(rol.Nombre) != null)
        // {
        //     throw new Exception("Ya existe un rol con ese nombre.");
        // }

        await _repository.AddAsync(rol);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateRoleAsync(Rol rol)
    {
        // El controlador ya verifica si existe,
        // pero podrías re-verificarlo aquí si quisieras.
        await _repository.UpdateAsync(rol);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteRoleAsync(int id)
    {
        // El controlador ya verifica si existe.
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
}