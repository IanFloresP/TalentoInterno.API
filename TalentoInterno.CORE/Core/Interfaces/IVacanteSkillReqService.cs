using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IVacanteSkillReqService
    {
        Task AddSkillToVacanteAsync(int vacanteId, VacanteSkillReqCreateDTO dto);
        Task<IEnumerable<VacanteSkillReqGetDTO>> GetSkillsByVacanteAsync(int vacanteId);
        Task<VacanteSkillReqGetDTO> GetVacanteSkillReqAsync(int vacanteId, int skillId);
        Task UpdateSkillOnVacanteAsync(int vacanteId, int skillId, VacanteSkillReqUpdateDTO dto);
    }
}