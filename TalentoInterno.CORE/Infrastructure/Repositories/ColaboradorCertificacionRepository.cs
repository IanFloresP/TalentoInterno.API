using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;



public class ColaboradorCertificacionRepository : IColaboradorCertificacionRepository
{
    private readonly TalentoInternooContext _context;

    public ColaboradorCertificacionRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ColaboradorCertificacion>> GetByColaboradorIdAsync(int colaboradorId)
    {
        return await _context.ColaboradorCertificacion
            .Include(cc => cc.Certificacion)
            .Where(cc => cc.ColaboradorId == colaboradorId)
            .ToListAsync();
    }

    public async Task AddAsync(ColaboradorCertificacion certificacion)
    {
        await _context.ColaboradorCertificacion.AddAsync(certificacion);
    }

    public async Task DeleteAsync(int colaboradorId, int certificacionId)
    {
        var item = await _context.ColaboradorCertificacion
            .FirstOrDefaultAsync(cc => cc.ColaboradorId == colaboradorId && cc.CertificacionId == certificacionId);

        if (item != null)
        {
            _context.ColaboradorCertificacion.Remove(item);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}