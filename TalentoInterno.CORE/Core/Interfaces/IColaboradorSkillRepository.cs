using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Infrastructure.Repositories
{
    public interface IColaboradorSkillRepository
    {
        Task AddSkillsAsync(IEnumerable<ColaboradorSkill> skills);
        Task DeleteAsync(int colaboradorId, int skillId);
        Task<IEnumerable<ColaboradorSkill>> GetByColaboradorIdAsync(int colaboradorId);
        Task<IEnumerable<Colaborador>> GetColaboradoresWithSkillsAsync(int? areaId, int? rolId);
        Task<ColaboradorSkill?> GetSingleAsync(int colaboradorId, int skillId);
        Task SaveChangesAsync();
        Task UpdateSkillAsync(ColaboradorSkill skill);
    }
}