using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface ISkillRepository
    {
        Task AddAsync(Skill skill);
        Task DeleteAsync(int id);
        Task<IEnumerable<Skill>> GetAllAsync();
        Task<Skill?> GetByIdAsync(int id);
        Task UpdateAsync(Skill skill);
    }
}