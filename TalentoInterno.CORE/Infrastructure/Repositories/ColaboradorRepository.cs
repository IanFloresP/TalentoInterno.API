using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class ColaboradorRepository : IColaboradorRepository
{
    private readonly TalentoInternooContext _context;

    public ColaboradorRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Colaborador>> GetAllAsync()
    {
        return await _context.Colaborador.ToListAsync();
    }

    public async Task<Colaborador?> GetByIdAsync(int id)
    {
        return await _context.Colaborador.FindAsync(id);
    }

    public async Task AddAsync(Colaborador colaborador)
    {
        await _context.Colaborador.AddAsync(colaborador);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Colaborador colaborador)
    {
        _context.Colaborador.Update(colaborador);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var colaborador = await GetByIdAsync(id);
        if (colaborador != null)
        {
            _context.Colaborador.Remove(colaborador);
            await _context.SaveChangesAsync();
        }
    }
}