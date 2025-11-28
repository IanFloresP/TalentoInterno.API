using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class PostulacionRepository : IPostulacionRepository
{
    private readonly TalentoInternooContext _context;

    public PostulacionRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<Postulacion?> GetByIdAsync(int id)
    {
        return await _context.Postulacion
            .Include(p => p.Vacante)
            .Include(p => p.Colaborador)
            .FirstOrDefaultAsync(p => p.PostulacionId == id);
    }

    public async Task<IEnumerable<Postulacion>> GetByVacanteIdAsync(int vacanteId)
    {
        return await _context.Postulacion
            .Include(p => p.Vacante)
            .Include(p => p.Colaborador)
            .Where(p => p.VacanteId == vacanteId)
            .OrderByDescending(p => p.MatchScore) // Ordenamos por mejor candidato
            .ToListAsync();
    }

    public async Task<IEnumerable<Postulacion>> GetByColaboradorIdAsync(int colaboradorId)
    {
        return await _context.Postulacion
            .Include(p => p.Vacante)
            .Where(p => p.ColaboradorId == colaboradorId)
            .ToListAsync();
    }

    public async Task AddAsync(Postulacion postulacion)
    {
        await _context.Postulacion.AddAsync(postulacion);
    }

    public Task UpdateAsync(Postulacion postulacion)
    {
        _context.Postulacion.Update(postulacion);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}