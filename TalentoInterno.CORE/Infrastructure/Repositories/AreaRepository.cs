using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class AreaRepository : IAreaRepository
{
    private readonly TalentoInternooContext _context;

    public AreaRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Area>> GetAllAsync()
    {
        // Incluimos el Departamento para que el servicio pueda usar el nombre
        return await _context.Area
            .Include(a => a.DepartamentoId)
            .ToListAsync();
    }

    public async Task<Area?> GetByIdAsync(int id)
    {
        return await _context.Area
            .Include(a => a.DepartamentoId)
            .FirstOrDefaultAsync(a => a.AreaId == id);
    }

    public async Task AddAsync(Area area)
    {
        await _context.Area.AddAsync(area);
    }

    public Task UpdateAsync(Area area)
    {
        _context.Area.Update(area);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var area = await GetByIdAsync(id);
        if (area != null)
        {
            _context.Area.Remove(area);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}