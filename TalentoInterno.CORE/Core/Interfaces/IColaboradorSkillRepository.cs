using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IColaboradorSkillRepository
    {
        Task AddSkillsAsync(IEnumerable<ColaboradorSkill> skills);
        Task<IEnumerable<ColaboradorSkill>> GetByColaboradorIdAsync(int colaboradorId);
        Task<IEnumerable<Colaborador>> GetColaboradoresWithSkillsAsync(int? areaId, int? rolId);
        Task<ColaboradorSkill?> GetSingleAsync(int colaboradorId, int skillId);
        Task<int> SaveChangesAsync();
        Task UpdateSkillAsync(ColaboradorSkill skill);
    }
}