using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class DepartamentoRepository : IDepartamentoRepository
{
    private readonly TalentoInternooContext _context;

    public DepartamentoRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Departamento>> GetAllAsync()
    {
        return await _context.Departamento.ToListAsync();
    }

    public async Task<Departamento?> GetByIdAsync(int id)
    {
        return await _context.Departamento.FindAsync(id);
    }

    public async Task AddAsync(Departamento departamento)
    {
        await _context.Departamento.AddAsync(departamento);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Departamento departamento)
    {
        _context.Departamento.Update(departamento);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var departamento = await GetByIdAsync(id);
        if (departamento != null)
        {
            _context.Departamento.Remove(departamento);
            await _context.SaveChangesAsync();
        }
    }
}
