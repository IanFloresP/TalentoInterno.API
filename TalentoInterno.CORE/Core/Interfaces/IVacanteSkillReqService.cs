using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    Task<IEnumerable<VacanteSkillReq>> GetAllVacanteSkillReqsAsync();
    Task<VacanteSkillReq?> GetVacanteSkillReqByIdAsync(int vacanteId, int skillId);
    Task<IEnumerable<VacanteSkillReq>> GetByVacanteIdAsync(int vacanteId);
    Task CreateVacanteSkillReqAsync(VacanteSkillReq vacanteSkillReq);
    Task UpdateVacanteSkillReqAsync(VacanteSkillReq vacanteSkillReq);
    Task DeleteVacanteSkillReqAsync(int vacanteId, int skillId);
}