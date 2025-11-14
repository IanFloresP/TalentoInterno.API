using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;
using System.Linq;

namespace TalentoInterno.CORE.Core.Services;

public class VacanteSkillReqService : IVacanteSkillReqService
{
    private readonly IVacanteSkillReqRepository _repository;

    public VacanteSkillReqService(IVacanteSkillReqRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<VacanteSkillReqGetDTO>> GetSkillsByVacanteAsync(int vacanteId)
    {
        var requisitos = await _repository.GetByVacanteIdAsync(vacanteId);

        return requisitos.Select(r => new VacanteSkillReqGetDTO
        {
            VacanteId = r.VacanteId,
            SkillId = r.SkillId,
            SkillNombre = r.Skill?.Nombre ?? "N/A",
            NivelDeseado = r.NivelDeseado,
            NivelNombre = r.NivelDeseadoNavigation?.Nombre ?? "N/A",
            Peso = r.Peso,
            Critico = r.Critico
        });
    }

    public async Task AddSkillToVacanteAsync(int vacanteId, VacanteSkillReqCreateDTO dto)
    {
        var vacanteSkillReq = new VacanteSkillReq
        {
            VacanteId = vacanteId,
            SkillId = dto.SkillId,
            NivelDeseado = (byte)dto.NivelDeseado,
            Peso = dto.Peso,
            Critico = dto.Critico
        };

        await _repository.AddAsync(vacanteSkillReq);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateSkillOnVacanteAsync(int vacanteId, int skillId, VacanteSkillReqUpdateDTO dto)
    {
        var existingReq = await _repository.GetByIdAsync(vacanteId, skillId);
        if (existingReq == null)
        {
            throw new KeyNotFoundException("El requisito de la vacante no fue encontrado.");
        }

        existingReq.NivelDeseado = (byte)dto.NivelDeseado;
        existingReq.Peso = dto.Peso;
        existingReq.Critico = dto.Critico;

        await _repository.UpdateAsync(existingReq);
        await _repository.SaveChangesAsync();
    }
}