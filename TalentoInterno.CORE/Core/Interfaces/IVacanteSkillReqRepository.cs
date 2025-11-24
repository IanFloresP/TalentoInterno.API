using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IVacanteSkillReqRepository
    {
        Task AddAsync(VacanteSkillReq vacanteSkillReq);
        Task DeleteAsync(int vacanteId, int skillId);
        Task<VacanteSkillReq?> GetByIdAsync(int vacanteId, int skillId);
        Task<IEnumerable<VacanteSkillReq>> GetByVacanteIdAsync(int vacanteId);
        Task SaveChangesAsync();
        Task UpdateAsync(VacanteSkillReq vacanteSkillReq);
    }
}