using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class TipoSkillService : ITipoSkillService
{
    private readonly ITipoSkillRepository _repository;

    public TipoSkillService(ITipoSkillRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TipoSkill>> GetAllTiposSkillAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TipoSkill?> GetTipoSkillByIdAsync(byte id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateTipoSkillAsync(TipoSkill tipoSkill)
    {
        await _repository.AddAsync(tipoSkill);
    }

    public async Task UpdateTipoSkillAsync(TipoSkill tipoSkill)
    {
        await _repository.UpdateAsync(tipoSkill);
    }

    public async Task DeleteTipoSkillAsync(byte id)
    {
        await _repository.DeleteAsync(id);
    }
}