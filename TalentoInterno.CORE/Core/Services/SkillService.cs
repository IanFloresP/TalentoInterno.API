using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs; // Importar

namespace TalentoInterno.CORE.Core.Services;

public class SkillService : ISkillService
{
    private readonly ISkillRepository _repository;

    public SkillService(ISkillRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Skill?> GetSkillByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    // --- MÉTODO MODIFICADO ---
    public async Task<Skill> CreateSkillAsync(SkillCreateDTO dto)
    {
        // Opcional: Puedes agregar validación para que no exista una skill con el mismo nombre
        // var existing = await _repository.GetByNameAsync(dto.Nombre);
        // if (existing != null) throw new Exception("Ya existe una skill con ese nombre.");

        var newSkill = new Skill
        {
            Nombre = dto.Nombre,
            TipoSkillId = dto.TipoSkillId,
            Critico = dto.Critico
        };

        // El repositorio guarda la entidad
        await _repository.AddAsync(newSkill);

        // Devolvemos la entidad con el nuevo SkillId
        return newSkill;
    }

    public async Task UpdateSkillAsync(Skill skill)
    {
        await _repository.UpdateAsync(skill);
    }

    public async Task DeleteSkillAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}