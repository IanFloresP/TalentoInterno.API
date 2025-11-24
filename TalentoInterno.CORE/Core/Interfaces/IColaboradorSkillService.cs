using TalentoInterno.CORE.Core.DTOs; // Importante para BrechaSkillDto
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IColaboradorSkillService
{
    Task<IEnumerable<ColaboradorSkill>> GetSkillsByColaboradorAsync(int colaboradorId);

    Task<IEnumerable<Colaborador>> GetColaboradoresWithSkillsAsync(int? areaId, int? rolId);

    Task<MatchResultDTO> GetMatchDetailsAsync(int colaboradorId, int vacanteId);

    // --- CAMBIO AQUÍ ---
    // Antes: Task<IEnumerable<object>> ...
    // Ahora: Devuelve el tipo fuerte 'BrechaSkillDto'
    Task<IEnumerable<BrechaSkillDto>> GetSkillGapsForVacanteAsync(int vacanteId, int? areaId = null);

    Task RegisterSkillsAsync(int colaboradorId, IEnumerable<ColaboradorSkillCreateDTO> skillsDTO);

    Task UpdateSkillAsync(int colaboradorId, int skillId, ColaboradorSkillUpdateDTO skillDTO);
}