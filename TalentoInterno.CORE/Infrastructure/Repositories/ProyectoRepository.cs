using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class ProyectoRepository : IProyectoRepository
{
    private readonly TalentoInternooContext _context;

    public ProyectoRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Proyecto>> GetAllAsync()
    {
        return await _context.Proyecto.ToListAsync();
    }

    public async Task<Proyecto?> GetByIdAsync(int id)
    {
        return await _context.Proyecto.FindAsync(id);
    }

    public async Task AddAsync(Proyecto proyecto)
    {
        await _context.Proyecto.AddAsync(proyecto);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Proyecto proyecto)
    {
        _context.Proyecto.Update(proyecto);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var proyecto = await GetByIdAsync(id);
        if (proyecto != null)
        {
            _context.Proyecto.Remove(proyecto);
            await _context.SaveChangesAsync();
        }
    }
}