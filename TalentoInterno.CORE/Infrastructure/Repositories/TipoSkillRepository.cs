using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class TipoSkillRepository : ITipoSkillRepository
{
    private readonly TalentoInternooContext _context;

    public TipoSkillRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TipoSkill>> GetAllAsync()
    {
        return await _context.TipoSkill.ToListAsync();
    }

    public async Task<TipoSkill?> GetByIdAsync(byte id)
    {
        return await _context.TipoSkill.FindAsync(id);
    }

    public async Task AddAsync(TipoSkill tipoSkill)
    {
        await _context.TipoSkill.AddAsync(tipoSkill);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TipoSkill tipoSkill)
    {
        _context.TipoSkill.Update(tipoSkill);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(byte id)
    {
        var tipoSkill = await GetByIdAsync(id);
        if (tipoSkill != null)
        {
            _context.TipoSkill.Remove(tipoSkill);
            await _context.SaveChangesAsync();
        }
    }
}