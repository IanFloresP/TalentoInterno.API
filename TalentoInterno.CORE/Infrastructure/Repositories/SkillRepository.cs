using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly TalentoInternooContext _context;

    public SkillRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Skill>> GetAllAsync()
    {
        return await _context.Skill.ToListAsync();
    }

    public async Task<Skill?> GetByIdAsync(int id)
    {
        return await _context.Skill.FindAsync(id);
    }

    public async Task AddAsync(Skill skill)
    {
        await _context.Skill.AddAsync(skill);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Skill skill)
    {
        _context.Skill.Update(skill);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var skill = await GetByIdAsync(id);
        if (skill != null)
        {
            _context.Skill.Remove(skill);
            await _context.SaveChangesAsync();
        }
    }
}