using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore; // ¡Importante para .Include() y .ToListAsync()!
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Infrastructure.Repositories;

public class VacanteSkillReqRepository : IVacanteSkillReqRepository
{
    private readonly TalentoInternooContext _context;

    public VacanteSkillReqRepository(TalentoInternooContext context)
    {
        _context = context;
    }

    // --- HU-09: Para el 'match' y 'VacanteController' ---
    // Este método es crucial y debe incluir las relaciones
    public async Task<IEnumerable<VacanteSkillReq>> GetByVacanteIdAsync(int vacanteId)
    {
        return await _context.VacanteSkillReq
            .Include(vsr => vsr.Skill)                 // ¡Incluye Skill!
            .Include(vsr => vsr.NivelDeseadoNavigation) // ¡Incluye Nivel!
            .Where(vsr => vsr.VacanteId == vacanteId)
            .ToListAsync();
    }

    // --- HU-06: Métodos CRUD ---
    public async Task<VacanteSkillReq?> GetByIdAsync(int vacanteId, int skillId)
    {
        // Busca por la llave primaria compuesta
        return await _context.VacanteSkillReq
            .FindAsync(vacanteId, skillId);
    }



    public async Task AddAsync(VacanteSkillReq vacanteSkillReq)
    {
        await _context.VacanteSkillReq.AddAsync(vacanteSkillReq);
    }

    public Task UpdateAsync(VacanteSkillReq vacanteSkillReq)
    {
        _context.VacanteSkillReq.Update(vacanteSkillReq);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int vacanteId, int skillId)
    {
        var req = await GetByIdAsync(vacanteId, skillId);
        if (req != null)
        {
            _context.VacanteSkillReq.Remove(req);
        }
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}