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
        return await _context.Colaborador
            .Include(c => c.Area)
                .ThenInclude(a => a.Departamento)
            .Include(c => c.Departamento)
            .Include(c => c.Rol)
            .Include(c => c.ColaboradorCertificacion)
                .ThenInclude(cc => cc.Certificacion)
            .Include(c => c.ColaboradorSkill)
                .ThenInclude(cs => cs.Skill)
            .Include(c => c.ColaboradorSkill)
                .ThenInclude(cs => cs.Nivel)
            .ToListAsync();
    }

    public async Task<Colaborador?> GetByIdAsync(int id)
    {
        return await _context.Colaborador
            .Include(c => c.Area)
            .Include(c => c.Departamento)
            .Include(c => c.Rol)
            .Include(c => c.ColaboradorCertificacion)
                .ThenInclude(cc => cc.Certificacion)
            .Include(c => c.ColaboradorSkill)
                .ThenInclude(cs => cs.Skill)
            .Include(c => c.ColaboradorSkill)
                .ThenInclude(cs => cs.Nivel)
            .FirstOrDefaultAsync(c => c.ColaboradorId == id);
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