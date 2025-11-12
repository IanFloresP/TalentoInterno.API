using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IVacanteSkillReqRepository
{
    Task<IEnumerable<VacanteSkillReq>> GetAllAsync();
    Task<VacanteSkillReq?> GetByIdAsync(int vacanteId, int skillId);
    Task AddAsync(VacanteSkillReq vacanteSkillReq);
    Task UpdateAsync(VacanteSkillReq vacanteSkillReq);
    Task DeleteAsync(int vacanteId, int skillId);
}