using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IVacanteSkillReqService
{
    Task<IEnumerable<VacanteSkillReq>> GetAllVacanteSkillReqsAsync();
    Task<VacanteSkillReq?> GetVacanteSkillReqByIdAsync(int vacanteId, int skillId);
    Task CreateVacanteSkillReqAsync(VacanteSkillReq vacanteSkillReq);
    Task UpdateVacanteSkillReqAsync(VacanteSkillReq vacanteSkillReq);
    Task DeleteVacanteSkillReqAsync(int vacanteId, int skillId);
}