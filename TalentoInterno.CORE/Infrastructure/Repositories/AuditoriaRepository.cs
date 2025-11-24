using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;
using System.Linq;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class AuditoriaRepository : IAuditoriaRepository
{
    private readonly TalentoInternooContext _context;

    public AuditoriaRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Auditoria auditoria)
    {
        await _context.Auditoria.AddAsync(auditoria);
    }

    public async Task<IEnumerable<Auditoria>> GetFiltradoAsync(int? usuarioId, DateTime? desde, DateTime? hasta, string? accion)
    {
        var query = _context.Auditoria
            .Include(a => a.Usuario) // Incluimos usuario para ver el email
            .AsQueryable();

        if (usuarioId.HasValue)
            query = query.Where(a => a.UsuarioId == usuarioId);

        if (desde.HasValue)
            query = query.Where(a => a.Fecha >= desde.Value);

        if (hasta.HasValue)
            query = query.Where(a => a.Fecha <= hasta.Value);

        if (!string.IsNullOrEmpty(accion))
            query = query.Where(a => a.Accion.Contains(accion));

        return await query.OrderByDescending(a => a.Fecha).ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
