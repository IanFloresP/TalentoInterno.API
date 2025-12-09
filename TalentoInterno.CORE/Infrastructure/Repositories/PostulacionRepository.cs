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

    public async Task<IEnumerable<Postulacion>> GetByEstadoAsync(string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
            return new List<Postulacion>();

        var e = estado.Trim().ToLower();

        IQueryable<Postulacion> query = _context.Postulacion
            .Include(p => p.Vacante)
            .Include(p => p.Colaborador);

        // 🔥 NO uses funciones propias aquí dentro del Where
        // ✅ Solo expresiones traducibles a SQL

        if (e.Contains("revisi")) // cubre "En Revision" y "En Revisión"
        {
            query = query.Where(p =>
                p.Estado != null &&
                (
                    p.Estado.Contains("Revision") ||
                    p.Estado.Contains("Revisión") ||
                    p.Estado.Contains("REVISION") ||
                    p.Estado.Contains("REVISIÓN")
                )
            );
        }
        else if (e.Contains("entrevista"))
        {
            query = query.Where(p =>
                p.Estado != null &&
                (p.Estado.Contains("Entrevista") || p.Estado.Contains("ENTREVISTA"))
            );
        }
        else if (e.Contains("seleccion"))
        {
            query = query.Where(p =>
                p.Estado != null &&
                (p.Estado.Contains("Seleccionado") || p.Estado.Contains("SELECCIONADO"))
            );
        }
        else if (e.Contains("rechaz"))
        {
            query = query.Where(p =>
                p.Estado != null &&
                (p.Estado.Contains("Rechazado") || p.Estado.Contains("RECHAZADO"))
            );
        }
        else
        {
            // fallback seguro
            query = query.Where(p => p.Estado != null && p.Estado.ToLower() == e);
        }

        return await query
            .OrderByDescending(p => p.MatchScore)
            .ToListAsync();
    }

    public async Task<IEnumerable<Postulacion>> GetAllByVacanteIdAsync(int vacanteId)
    {
        return await _context.Postulacion
            .Where(p => p.VacanteId == vacanteId)
            .ToListAsync();
    }



}