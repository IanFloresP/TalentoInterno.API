using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface ITipoSkillRepository
{
    Task<IEnumerable<TipoSkill>> GetAllAsync();
    Task<TipoSkill?> GetByIdAsync(byte id);
    Task AddAsync(TipoSkill tipoSkill);
    Task UpdateAsync(TipoSkill tipoSkill);
    Task DeleteAsync(byte id);
}