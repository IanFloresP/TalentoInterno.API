using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class VacanteSkillReqService : IVacanteSkillReqService
{
    private readonly IVacanteSkillReqRepository _repository;

    public VacanteSkillReqService(IVacanteSkillReqRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<VacanteSkillReq>> GetAllVacanteSkillReqsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<VacanteSkillReq?> GetVacanteSkillReqByIdAsync(int vacanteId, int skillId)
    {
        return await _repository.GetByIdAsync(vacanteId, skillId);
    }

    public async Task CreateVacanteSkillReqAsync(VacanteSkillReq vacanteSkillReq)
    {
        await _repository.AddAsync(vacanteSkillReq);
    }

    public async Task UpdateVacanteSkillReqAsync(VacanteSkillReq vacanteSkillReq)
    {
        await _repository.UpdateAsync(vacanteSkillReq);
    }

    public async Task DeleteVacanteSkillReqAsync(int vacanteId, int skillId)
    {
        await _repository.DeleteAsync(vacanteId, skillId);
    }
}