using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IColaboradorSkillRepository
{
    Task<IEnumerable<ColaboradorSkill>> GetByColaboradorIdAsync(int colaboradorId);
    Task<IEnumerable<Colaborador>> GetColaboradoresWithSkillsAsync(int? areaId, int? rolId);
}
