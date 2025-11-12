using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class CuentaRepository : ICuentaRepository
{
    private readonly TalentoInternooContext _context;

    public CuentaRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cuenta>> GetAllAsync()
    {
        return await _context.Cuenta.ToListAsync();
    }

    public async Task<Cuenta?> GetByIdAsync(int id)
    {
        return await _context.Cuenta.FindAsync(id);
    }

    public async Task AddAsync(Cuenta cuenta)
    {
        await _context.Cuenta.AddAsync(cuenta);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cuenta cuenta)
    {
        _context.Cuenta.Update(cuenta);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var cuenta = await GetByIdAsync(id);
        if (cuenta != null)
        {
            _context.Cuenta.Remove(cuenta);
            await _context.SaveChangesAsync();
        }
    }
}