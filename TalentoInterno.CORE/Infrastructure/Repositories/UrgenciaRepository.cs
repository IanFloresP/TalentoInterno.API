using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class UrgenciaRepository : IUrgenciaRepository
{
    private readonly TalentoInternooContext _context;

    public UrgenciaRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Urgencia>> GetAllAsync()
    {
        return await _context.Urgencia.ToListAsync();
    }

    public async Task<Urgencia?> GetByIdAsync(byte id)
    {
        return await _context.Urgencia.FindAsync(id);
    }

    public async Task AddAsync(Urgencia urgencia)
    {
        await _context.Urgencia.AddAsync(urgencia);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Urgencia urgencia)
    {
        _context.Urgencia.Update(urgencia);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(byte id)
    {
        var urgencia = await GetByIdAsync(id);
        if (urgencia != null)
        {
            _context.Urgencia.Remove(urgencia);
            await _context.SaveChangesAsync();
        }
    }
}