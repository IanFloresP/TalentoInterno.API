using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

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

    public async Task CreateSkillAsync(Skill skill)
    {
        await _repository.AddAsync(skill);
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