using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IColaboradorSkillService
    {
        Task<IEnumerable<Colaborador>> GetColaboradoresWithSkillsAsync(int? areaId, int? rolId);
        Task<MatchResultDTO> GetMatchDetailsAsync(int colaboradorId, int vacanteId);
        Task<IEnumerable<object>> GetSkillGapsForVacanteAsync(int vacanteId, int? areaId);
        Task<IEnumerable<ColaboradorSkill>> GetSkillsByColaboradorAsync(int colaboradorId);
        Task RegisterSkillsAsync(int colaboradorId, IEnumerable<ColaboradorSkillCreateDTO> skillsDTO);
        Task UpdateSkillAsync(int colaboradorId, int skillId, ColaboradorSkillUpdateDTO skillDTO);
    }
}