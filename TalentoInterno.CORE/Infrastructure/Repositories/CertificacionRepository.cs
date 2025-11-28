using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class CertificacionRepository : ICertificacionRepository
{
    private readonly TalentoInternooContext _context;

    public CertificacionRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Certificacion>> GetAllAsync()
    {
        return await _context.Certificacion.ToListAsync();
    }

    public async Task<Certificacion?> GetByIdAsync(int id)
    {
        return await _context.Certificacion.FindAsync(id);
    }

    public async Task AddAsync(Certificacion certificacion)
    {
        await _context.Certificacion.AddAsync(certificacion);
    }

    public Task UpdateAsync(Certificacion certificacion)
    {
        _context.Certificacion.Update(certificacion);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var certificacion = await GetByIdAsync(id);
        if (certificacion != null)
        {
            _context.Certificacion.Remove(certificacion);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
