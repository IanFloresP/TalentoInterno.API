using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface ITipoSkillService
{
    Task<IEnumerable<TipoSkill>> GetAllTiposSkillAsync();
    Task<TipoSkill?> GetTipoSkillByIdAsync(byte id);
    Task CreateTipoSkillAsync(TipoSkill tipoSkill);
    Task UpdateTipoSkillAsync(TipoSkill tipoSkill);
    Task DeleteTipoSkillAsync(byte id);
}