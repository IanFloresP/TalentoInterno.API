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
        return await _context.Vacante
            .Include(v => v.Perfil)     // <-- ¡AÑADE ESTO!
            .Include(v => v.Urgencia)   // <-- ¡AÑADE ESTO!
            .ToListAsync();
    }


    public async Task<Vacante?> GetByIdAsync(int id)
    {
        return await _context.Vacante
            .Include(v => v.Perfil)
            .Include(v => v.Cuenta)
            .Include(v => v.Proyecto)
            .Include(v => v.Urgencia)
            .FirstOrDefaultAsync(v => v.VacanteId == id);
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