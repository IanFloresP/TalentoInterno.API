using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class PerfilRepository : IPerfilRepository
{
    private readonly TalentoInternooContext _context;

    public PerfilRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Perfil>> GetAllAsync()
    {
        return await _context.Perfil.ToListAsync();
    }

    public async Task<Perfil?> GetByIdAsync(int id)
    {
        return await _context.Perfil.FindAsync(id);
    }

    public async Task AddAsync(Perfil perfil)
    {
        await _context.Perfil.AddAsync(perfil);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Perfil perfil)
    {
        _context.Perfil.Update(perfil);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var perfil = await GetByIdAsync(id);
        if (perfil != null)
        {
            _context.Perfil.Remove(perfil);
            await _context.SaveChangesAsync();
        }
    }
}