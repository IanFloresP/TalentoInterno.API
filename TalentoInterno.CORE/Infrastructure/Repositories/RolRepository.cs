using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class RolRepository : IRolRepository
{
    private readonly TalentoInternooContext _context;

    public RolRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Rol>> GetAllAsync()
    {
        return await _context.Rol.ToListAsync();
    }

    public async Task<Rol?> GetByIdAsync(int id)
    {
        return await _context.Rol.FindAsync(id);
    }

    public async Task AddAsync(Rol rol)
    {
        await _context.Rol.AddAsync(rol);
    }

    public Task UpdateAsync(Rol rol)
    {
        _context.Rol.Update(rol);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var rol = await GetByIdAsync(id);
        if (rol != null)
        {
            _context.Rol.Remove(rol);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}