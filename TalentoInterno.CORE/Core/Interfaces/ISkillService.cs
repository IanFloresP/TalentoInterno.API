using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface ISkillService
{
    Task<IEnumerable<Skill>> GetAllSkillsAsync();
    Task<Skill?> GetSkillByIdAsync(int id);
    Task<Skill> CreateSkillAsync(SkillCreateDTO dto);
    Task UpdateSkillAsync(Skill skill);
    Task DeleteSkillAsync(int id);
}