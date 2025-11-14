using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/ColaboradorSkill")]
public class ColaboradorSkillController : ControllerBase
{
    private readonly IColaboradorSkillService _colaboradorSkillService;

    public ColaboradorSkillController(IColaboradorSkillService colaboradorSkillService)
    {
        _colaboradorSkillService = colaboradorSkillService;
    }

    [HttpGet("{colaboradorId}/skills")]
    public async Task<IActionResult> GetSkills(int colaboradorId)
    {
        var skills = await _colaboradorSkillService.GetSkillsByColaboradorAsync(colaboradorId);
        var dto = skills.Select(s => new ColaboradorSkillDto
        {
            ColaboradorId = s.ColaboradorId,
            SkillId = s.SkillId,
            SkillNombre = s.Skill?.Nombre,
            NivelId = s.NivelId,
            NivelNombre = s.Nivel?.Nombre,
            AniosExp = s.AniosExp
        });
        return Ok(dto);
    }

    [HttpGet("mapping")]
    public async Task<IActionResult> GetMapping([FromQuery] int? areaId, [FromQuery] int? rolId)
    {
        var colaboradores = await _colaboradorSkillService.GetColaboradoresWithSkillsAsync(areaId, rolId);
        var result = colaboradores.Select(c => new ColaboradorMappingDto
        {
            ColaboradorId = c.ColaboradorId,
            Nombre = $"{c.Nombres} {c.Apellidos}",
            Rol = c.Rol?.Nombre,
            Skills = c.ColaboradorSkill.Select(cs => new ColaboradorSkillDto
            {
                ColaboradorId = cs.ColaboradorId,
                SkillId = cs.SkillId,
                SkillNombre = cs.Skill?.Nombre,
                NivelId = cs.NivelId,
                NivelNombre = cs.Nivel?.Nombre,
                AniosExp = cs.AniosExp
            }).ToList()
        });
        return Ok(result);
    }

    [HttpGet("{colaboradorId}/match/{vacanteId}")]
    public async Task<IActionResult> GetMatch(int colaboradorId, int vacanteId)
    {
        var skills = await _colaboradorSkillService.GetSkillsByColaboradorAsync(colaboradorId);
        var dto = skills.Select(s => new ColaboradorSkillDto
        {
            ColaboradorId = s.ColaboradorId,
            SkillId = s.SkillId,
            SkillNombre = s.Skill?.Nombre,
            NivelId = s.NivelId,
            NivelNombre = s.Nivel?.Nombre,
            AniosExp = s.AniosExp
        });
        return Ok(new { ColaboradorId = colaboradorId, VacanteId = vacanteId, Skills = dto });
    }

    [HttpGet("vacante/{vacanteId}/gaps")]
    public async Task<IActionResult> GetGaps(int vacanteId, [FromQuery] int? areaId)
    {
        var gaps = await _colaboradorSkillService.GetSkillGapsForVacanteAsync(vacanteId, areaId);
        // convert to DTOs
        var dto = gaps.Select(g => new SkillGapDto
        {
            SkillId = (int)g.GetType().GetProperty("SkillId")!.GetValue(g)!,
            SkillNombre = (string)g.GetType().GetProperty("SkillNombre")!.GetValue(g)!,
            NivelDeseado = (byte)g.GetType().GetProperty("NivelDeseado")!.GetValue(g)!,
            AvailableCount = (int)g.GetType().GetProperty("AvailableCount")!.GetValue(g)!,
            Gap = (int)g.GetType().GetProperty("Gap")!.GetValue(g)!,
            Critico = (bool)g.GetType().GetProperty("Critico")!.GetValue(g)!,
            RecruitmentAlert = (bool)g.GetType().GetProperty("RecruitmentAlert")!.GetValue(g)!
        });
        return Ok(dto);
    }
}