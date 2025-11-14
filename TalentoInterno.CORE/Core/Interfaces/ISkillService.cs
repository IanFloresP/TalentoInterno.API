using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface ISkillService
    {
        Task<Skill> CreateSkillAsync(SkillCreateDTO dto);
        Task DeleteSkillAsync(int id);
        Task<IEnumerable<Skill>> GetAllSkillsAsync();
        Task<Skill?> GetSkillByIdAsync(int id);
        Task UpdateSkillAsync(Skill skill);
    }
}