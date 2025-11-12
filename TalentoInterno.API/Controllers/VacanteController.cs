using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Services;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VacanteController : ControllerBase
{
    private readonly IVacanteService _vacanteService;

    public VacanteController(IVacanteService vacanteService)
    {
        _vacanteService = vacanteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vacantes = await _vacanteService.GetAllVacantesAsync();
        return Ok(vacantes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vacante = await _vacanteService.GetVacanteByIdAsync(id);
        if (vacante == null)
            return NotFound();
        return Ok(vacante);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VacanteDto vacanteDto)
    {
        await _vacanteService.CreateVacanteAsync(new Vacante
        {
            // Map properties from DTO to entity
        });
        return CreatedAtAction(nameof(GetById), new { id = vacanteDto.VacanteId }, vacanteDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] VacanteDto vacanteDto)
    {
        var vacante = await _vacanteService.GetVacanteByIdAsync(id);
        if (vacante == null)
            return NotFound();

        // Map properties from DTO to entity
        await _vacanteService.UpdateVacanteAsync(vacante);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vacante = await _vacanteService.GetVacanteByIdAsync(id);
        if (vacante == null)
            return NotFound();

        await _vacanteService.DeleteVacanteAsync(id);
        return NoContent();
    }

    [HttpGet("{id}/skills")]
    public IActionResult GetSkills(int id)
    {
        // Implement logic for HU-09
        return Ok();
    }

    [HttpGet("{id}/matching")]
    public IActionResult GetMatchingCandidates(int id)
    {
        // Implement logic for HU-09, HU-10
        return Ok();
    }

    [HttpGet("{id}/ranking")]
    public IActionResult GetRanking(int id)
    {
        // Implement logic for HU-09, HU-13
        return Ok();
    }
}