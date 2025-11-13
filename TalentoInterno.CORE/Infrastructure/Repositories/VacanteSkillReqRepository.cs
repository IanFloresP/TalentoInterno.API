using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class VacanteSkillReqRepository : IVacanteSkillReqRepository
{
    private readonly TalentoInternooContext _context;

    public VacanteSkillReqRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VacanteSkillReq>> GetAllAsync()
    {
        return await _context.VacanteSkillReq.ToListAsync();
    }

    public async Task<VacanteSkillReq?> GetByIdAsync(int vacanteId, int skillId)
    {
        return await _context.VacanteSkillReq.FindAsync(vacanteId, skillId);
    }

    public async Task<IEnumerable<VacanteSkillReq>> GetByVacanteIdAsync(int vacanteId)
    {
        return await _context.VacanteSkillReq
            .Include(v => v.Skill)
            .Include(v => v.NivelDeseadoNavigation)
            .Where(v => v.VacanteId == vacanteId)
            .ToListAsync();
    }

    public async Task AddAsync(VacanteSkillReq vacanteSkillReq)
    {
        await _context.VacanteSkillReq.AddAsync(vacanteSkillReq);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(VacanteSkillReq vacanteSkillReq)
    {
        _context.VacanteSkillReq.Update(vacanteSkillReq);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int vacanteId, int skillId)
    {
        var vacanteSkillReq = await GetByIdAsync(vacanteId, skillId);
        if (vacanteSkillReq != null)
        {
            _context.VacanteSkillReq.Remove(vacanteSkillReq);
            await _context.SaveChangesAsync();
        }
    }
}