using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs; // ¡Importar DTOs!
using System.Linq; // ¡Importar LINQ!

namespace TalentoInterno.CORE.Core.Services;

public class VacanteSkillReqService : IVacanteSkillReqService
{
    private readonly IVacanteSkillReqRepository _repository;

    public VacanteSkillReqService(IVacanteSkillReqRepository repository)
    {
        _repository = repository;
    }

    // --- Implementación de GET (para VacanteController) ---
    public async Task<IEnumerable<VacanteSkillReqGetDTO>> GetSkillsByVacanteAsync(int vacanteId)
    {
        // 1. Llamamos al repositorio
        var requisitos = await _repository.GetByVacanteIdAsync(vacanteId);

        // 2. Mapeamos la Entidad al DTO "Rico"
        return requisitos.Select(r => new VacanteSkillReqGetDTO
        {
            VacanteId = r.VacanteId,
            SkillId = r.SkillId,
            SkillNombre = r.Skill?.Nombre ?? "N/A", // Usamos ?. por seguridad
            NivelDeseado = r.NivelDeseado,
            NivelNombre = r.NivelDeseadoNavigation?.Nombre ?? "N/A", // Usamos ?.
            Peso = r.Peso,
            Critico = r.Critico
        });
    }

    // --- Implementación de POST (para VacanteSkillReqController) ---
    public async Task AddSkillToVacanteAsync(int vacanteId, VacanteSkillReqCreateDTO dto)
    {
        // 1. Mapeamos el DTO a la Entidad
        var vacanteSkillReq = new VacanteSkillReq
        {
            VacanteId = vacanteId,
            SkillId = dto.SkillId,
            NivelDeseado = dto.NivelDeseado,
            Peso = dto.Peso,
            Critico = dto.Critico
        };

        // 2. Llamamos al repositorio
        await _repository.AddAsync(vacanteSkillReq);
        await _repository.SaveChangesAsync(); // Guardamos
    }

    // --- Implementación de PUT (para VacanteSkillReqController) ---
    public async Task UpdateSkillOnVacanteAsync(int vacanteId, int skillId, VacanteSkillReqUpdateDTO dto)
    {
        // 1. Obtenemos la entidad existente
        var existingReq = await _repository.GetByIdAsync(vacanteId, skillId);
        if (existingReq == null)
        {
            throw new KeyNotFoundException("El requisito de la vacante no fue encontrado.");
        }

        // 2. Mapeamos el DTO a la entidad
        existingReq.NivelDeseado = dto.NivelDeseado;
        existingReq.Peso = dto.Peso;
        existingReq.Critico = dto.Critico;

        // 3. Llamamos al repositorio
        await _repository.UpdateAsync(existingReq);
        await _repository.SaveChangesAsync(); // Guardamos
    }
}