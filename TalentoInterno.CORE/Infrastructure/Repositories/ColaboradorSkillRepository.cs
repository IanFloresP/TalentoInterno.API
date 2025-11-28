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
                .ThenInclude(s => s.TipoSkill) 
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
            .Include(c => c.ColaboradorSkill).ThenInclude(cs => cs.Nivel)
            .AsQueryable();

        if (areaId.HasValue)
            query = query.Where(c => c.AreaId == areaId.Value);
        if (rolId.HasValue)
            query = query.Where(c => c.RolId == rolId.Value);

        return await query.ToListAsync();
    }

    public async Task<ColaboradorSkill?> GetSingleAsync(int colaboradorId, int skillId)
    {
        return await _context.ColaboradorSkill
            .FirstOrDefaultAsync(cs => cs.ColaboradorId == colaboradorId && cs.SkillId == skillId);
    }

    public async Task AddSkillsAsync(IEnumerable<ColaboradorSkill> skills)
    {
        await _context.ColaboradorSkill.AddRangeAsync(skills);
    }

    public Task UpdateSkillAsync(ColaboradorSkill skill)
    {
        _context.ColaboradorSkill.Update(skill);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int colaboradorId, int skillId)
    {
        var skill = await GetSingleAsync(colaboradorId, skillId);
        if (skill != null)
        {
            _context.ColaboradorSkill.Remove(skill);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}