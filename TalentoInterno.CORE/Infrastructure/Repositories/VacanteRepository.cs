using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class VacanteRepository : IVacanteRepository
{
    private readonly TalentoInternooContext _context;

    public VacanteRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vacante>> GetAllAsync()
    {
        return await _context.Vacante.ToListAsync();
    }

    public async Task<Vacante?> GetByIdAsync(int id)
    {
        return await _context.Vacante.FindAsync(id);
    }

    public async Task AddAsync(Vacante vacante)
    {
        await _context.Vacante.AddAsync(vacante);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vacante vacante)
    {
        _context.Vacante.Update(vacante);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var vacante = await GetByIdAsync(id);
        if (vacante != null)
        {
            _context.Vacante.Remove(vacante);
            await _context.SaveChangesAsync();
        }
    }
}