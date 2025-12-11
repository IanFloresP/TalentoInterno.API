using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoInterno.API.Filters;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Services;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin, RRHH, Business Manager")]
[Route("api/[controller]")]
public class VacanteController : ControllerBase
{
    private readonly IVacanteService _vacanteService;
    private readonly IColaboradorSkillService _colaboradorSkillService;
    private readonly IVacanteSkillReqService _vacanteSkillReqService;

    public VacanteController(IVacanteService vacanteService, IColaboradorSkillService colaboradorSkillService, IVacanteSkillReqService vacanteSkillReqService)
    {
        _vacanteService = vacanteService;
        _colaboradorSkillService = colaboradorSkillService;
        _vacanteSkillReqService = vacanteSkillReqService;
    }

    // HU-12: Listar todo
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? perfilId) 
    {
        var vacantes = await _vacanteService.GetAllVacantesAsync(perfilId);
        return Ok(vacantes);
    }

    // HU-08: Ver detallado
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vacante = await _vacanteService.GetVacanteByIdAsync(id);
        if (vacante == null) return NotFound();
        return Ok(vacante);
    }

    // HU-06: Registrar vacante Y sus skills
    [HttpPost]
    [Authorize(Roles = "Admin, RRHH")]
    [ServiceFilter(typeof(AuditoriaFilter))]
    public async Task<IActionResult> Create([FromBody] VacanteCreateDTO dto)
    {
        try
        {
            var nuevaVacante = await _vacanteService.CreateVacanteAsync(dto);
            var vacanteDto = await _vacanteService.GetVacanteByIdAsync(nuevaVacante.VacanteId);
            return CreatedAtAction(nameof(GetById), new { id = nuevaVacante.VacanteId }, vacanteDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Error al crear la vacante.", details = ex.Message });
        }
    }

    // Actualizar datos de la vacante (no sus skills)
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, RRHH")]
    [ServiceFilter(typeof(AuditoriaFilter))]

    public async Task<IActionResult> Update(int id, [FromBody] VacanteUpdateDTO dto)
    {
        try
        {
            await _vacanteService.UpdateVacanteAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, RRHH")]
    [ServiceFilter(typeof(AuditoriaFilter))]
    public async Task<IActionResult> Delete(int id)
    {
        await _vacanteService.DeleteVacanteAsync(id);
        return NoContent();
    }

    // HU-09: Obtener habilidades requisitos de la vacante
    [HttpGet("{id}/skills")]
    public async Task<IActionResult> GetSkills(int id)
    {
        // Return the skill requirements for the vacante
        var reqs = await _vacanteSkillReqService.GetSkillsByVacanteAsync(id);
        var dto = reqs.Select(r => new VacanteSkillReqDto
        {
            VacanteId = r.VacanteId,
            SkillId = r.SkillId,
            SkillName = r.SkillNombre,
            NivelDeseado = r.NivelDeseado,
            Peso = r.Peso,
            Critico = r.Critico
        });
        return Ok(dto);
    }

}