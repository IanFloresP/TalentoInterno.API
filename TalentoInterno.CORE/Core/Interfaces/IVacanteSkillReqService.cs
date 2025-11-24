using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Services
{
    public interface IVacanteSkillReqService
    {
        Task AddSkillToVacanteAsync(int vacanteId, VacanteSkillReqCreateDTO dto);
        Task<IEnumerable<VacanteSkillReqGetDTO>> GetSkillsByVacanteAsync(int vacanteId);
        Task UpdateSkillOnVacanteAsync(int vacanteId, int skillId, VacanteSkillReqUpdateDTO dto);
    }
}