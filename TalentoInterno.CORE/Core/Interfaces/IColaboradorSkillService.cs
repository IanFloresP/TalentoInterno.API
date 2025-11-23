using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IColaboradorSkillService
{
    Task<IEnumerable<ColaboradorSkill>> GetSkillsByColaboradorAsync(int colaboradorId);
    Task<IEnumerable<Colaborador>> GetColaboradoresWithSkillsAsync(int? areaId, int? rolId);
    Task<IEnumerable<object>> GetSkillGapsForVacanteAsync(int vacanteId, int? areaId);
}
