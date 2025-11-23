using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IVacanteSkillReqRepository
{
    // Para el 'match' y 'VacanteController' (HU-09)
    Task<IEnumerable<VacanteSkillReq>> GetByVacanteIdAsync(int vacanteId);

    // Para el CRUD de VacanteSkillReqController (HU-06)
    Task<VacanteSkillReq?> GetByIdAsync(int vacanteId, int skillId);
    Task<IEnumerable<VacanteSkillReq>> GetByVacanteIdAsync(int vacanteId);
    Task AddAsync(VacanteSkillReq vacanteSkillReq);
    Task UpdateAsync(VacanteSkillReq vacanteSkillReq);
    Task DeleteAsync(int vacanteId, int skillId);

    // Para guardar cambios desde el servicio
    Task SaveChangesAsync();
}