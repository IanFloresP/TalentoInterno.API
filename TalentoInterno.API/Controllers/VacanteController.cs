using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Services;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VacanteController : ControllerBase
{
    private readonly IVacanteService _vacanteService;
    private readonly IVacanteSkillReqService _vacanteSkillReqService; // Para GET /skills
    private readonly IMatchingService _matchingService; // Para /matching y /ranking

    public VacanteController(
        IVacanteService vacanteService,
        IVacanteSkillReqService vacanteSkillReqService,
        IMatchingService matchingService)
    {
        _vacanteService = vacanteService;
        _vacanteSkillReqService = vacanteSkillReqService;
        _matchingService = matchingService;
    }

    // HU-12: Listar todo
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vacantes = await _vacanteService.GetAllVacantesAsync();
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
    public async Task<IActionResult> Delete(int id)
    {
        await _vacanteService.DeleteVacanteAsync(id);
        return NoContent();
    }

    // HU-09: Obtener habilidades requisitos de la vacante
    [HttpGet("{id}/skills")]
    public async Task<IActionResult> GetSkills(int id)
    {
        var skills = await _vacanteSkillReqService.GetSkillsByVacanteAsync(id);
        return Ok(skills);
    }

    // HU-07, HU-08, HU-10: Ejecutar algoritmo de matching
    [HttpGet("{id}/matching")]
    public async Task<IActionResult> GetMatchingCandidates(int id)
    {
        var ranking = await _matchingService.GetRankedCandidatesAsync(id);
        return Ok(ranking);
    }

    // HU-08, HU-13: Visualizar clasificación de candidatos
    [HttpGet("{id}/ranking")]
    public async Task<IActionResult> GetRanking(int id)
    {
        var ranking = await _matchingService.GetRankedCandidatesAsync(id);
        return Ok(ranking);
    }
}