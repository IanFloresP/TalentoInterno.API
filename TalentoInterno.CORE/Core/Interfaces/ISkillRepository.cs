using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface ISkillRepository
{
    Task<IEnumerable<Skill>> GetAllAsync();
    Task<Skill?> GetByIdAsync(int id);
    Task AddAsync(Skill skill);
    Task UpdateAsync(Skill skill);
    Task DeleteAsync(int id);
}