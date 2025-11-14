using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;

namespace TalentoInterno.CORE.Core.Services;

public class SkillService : ISkillService
{
    private readonly ISkillRepository _repository;
    private readonly TalentoInternooContext _context;

    public SkillService(ISkillRepository repository, TalentoInternooContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Skill?> GetSkillByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Skill> CreateSkillAsync(SkillCreateDTO dto)
    {
        var newSkill = new Skill
        {
            Nombre = dto.Nombre,
            TipoSkillId = dto.TipoSkillId,
            Critico = dto.Critico
        };

        // 1. Añade la skill a la base de datos (se genera el ID)
        await _repository.AddAsync(newSkill);

        // 2. CORRECCIÓN: Carga la Entidad TipoSkill usando el ID que acaba de ser guardado
        //    y lo asigna a la propiedad de navegación de la nueva skill.
        var tipoSkill = await _context.TipoSkill.FindAsync(newSkill.TipoSkillId);
        newSkill.TipoSkill = tipoSkill; // Esto hace que newSkill.TipoSkill ya no sea null

        // 3. Devuelve la entidad completa
        return newSkill;
    }

    // ... (Tus otros métodos Update y Delete) ...


    public async Task UpdateSkillAsync(Skill skill)
    {
        await _repository.UpdateAsync(skill);
    }

    public async Task DeleteSkillAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}