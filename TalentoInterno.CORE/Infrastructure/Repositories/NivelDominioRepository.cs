using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class NivelDominioRepository : INivelDominioRepository
{
    private readonly TalentoInternooContext _context;

    public NivelDominioRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NivelDominio>> GetAllAsync()
    {
        return await _context.NivelDominio.ToListAsync();
    }

    public async Task<NivelDominio?> GetByIdAsync(byte id)
    {
        return await _context.NivelDominio.FindAsync(id);
    }

    public async Task AddAsync(NivelDominio nivelDominio)
    {
        await _context.NivelDominio.AddAsync(nivelDominio);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(NivelDominio nivelDominio)
    {
        _context.NivelDominio.Update(nivelDominio);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(byte id)
    {
        var nivelDominio = await GetByIdAsync(id);
        if (nivelDominio != null)
        {
            _context.NivelDominio.Remove(nivelDominio);
            await _context.SaveChangesAsync();
        }
    }
}