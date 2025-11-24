using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;

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

    [HttpPost("{colaboradorId}/skills")]
    [Authorize(Roles = "Admin, RRHH")]
    public async Task<IActionResult> RegistrarSkills(int colaboradorId, [FromBody] List<ColaboradorSkillCreateDTO> skillsDTO)
    {
        await _colaboradorSkillService.RegisterSkillsAsync(colaboradorId, skillsDTO);
        return Ok(new { message = "Skills registradas exitosamente" });
    }

    [HttpPut("{colaboradorId}/skills/{skillId}")]
    [Authorize(Roles = "Admin, RRHH")]
    public async Task<IActionResult> UpdateColaboradorSkill(int colaboradorId, int skillId, [FromBody] ColaboradorSkillUpdateDTO skillDTO)
    {
        try
        {
            await _colaboradorSkillService.UpdateSkillAsync(colaboradorId, skillId, skillDTO);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{colaboradorId}/match/{vacanteId}")]
    public async Task<IActionResult> GetMatch(int colaboradorId, int vacanteId)
    {
        var matchResult = await _colaboradorSkillService.GetMatchDetailsAsync(colaboradorId, vacanteId);
        return Ok(matchResult);
    }

    [HttpGet("mapping")]
    public async Task<IActionResult> GetMapping([FromQuery] int? areaId, [FromQuery] int? rolId)
    {
        var colaboradores = await _colaboradorSkillService.GetColaboradoresWithSkillsAsync(areaId, rolId);
        // ... (Tu lógica de mapeo a ColaboradorMappingDto va aquí)
        return Ok(colaboradores);
    }

    [HttpGet("vacante/{vacanteId}/gaps")]
    public async Task<IActionResult> GetGaps(int vacanteId, [FromQuery] int? areaId)
    {
        var gaps = await _colaboradorSkillService.GetSkillGapsForVacanteAsync(vacanteId);
        // ... (Tu lógica de mapeo a SkillGapDto va aquí)
        return Ok(gaps);
    }
}