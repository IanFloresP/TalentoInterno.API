using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class ColaboradorSkillRepository : IColaboradorSkillRepository
{
    private readonly TalentoInternooContext _context;

    public ColaboradorSkillRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ColaboradorSkill>> GetByColaboradorIdAsync(int colaboradorId)
    {
        return await _context.ColaboradorSkill
            .Include(cs => cs.Skill)
            .Include(cs => cs.Nivel)
            .Where(cs => cs.ColaboradorId == colaboradorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Colaborador>> GetColaboradoresWithSkillsAsync(int? areaId, int? rolId)
    {
        var query = _context.Colaborador
            .Include(c => c.Rol)
            .Include(c => c.Area)
            .Include(c => c.ColaboradorSkill).ThenInclude(cs => cs.Skill)
            .AsQueryable();

        if (areaId.HasValue)
            query = query.Where(c => c.AreaId == areaId.Value);
        if (rolId.HasValue)
            query = query.Where(c => c.RolId == rolId.Value);

        return await query.ToListAsync();
    }
}
