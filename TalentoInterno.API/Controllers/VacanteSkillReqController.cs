using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.DTOs; // ¡Aquí están los DTOs correctos!
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin, RRHH, Business Manager")]
[Route("api/VacanteSkillReq")] // Ruta base
public class VacanteSkillReqController : ControllerBase
{
    private readonly IVacanteSkillReqService _vacanteSkillReqService;

    public VacanteSkillReqController(IVacanteSkillReqService vacanteSkillReqService)
    {
        _vacanteSkillReqService = vacanteSkillReqService;
    }

    // -----------------------------------------------------------------
    // NOTA: El [HttpGet("{vacanteId}/skills")] (HU-09) 
    // ya lo implementamos en 'VacanteController'. 
    // No debe estar aquí también para evitar conflictos.
    // -----------------------------------------------------------------


    // --- HU-06: Agregar habilidad requerida a una vacante ---
    [HttpPost("{vacanteId}/skills")]
    [Authorize(Roles = "Admin, RRHH")]
    public async Task<IActionResult> AddSkillToVacante(int vacanteId, [FromBody] VacanteSkillReqCreateDTO dto)
    {
        try
        {
            await _vacanteSkillReqService.AddSkillToVacanteAsync(vacanteId, dto);
            return Ok(new { message = "Habilidad agregada a la vacante." });
        }
        catch (Exception ex)
        {
            // (Manejo de error, ej: si la skill ya existe en la vacante)
            return Conflict(new { message = ex.Message });
        }
    }

    // --- HU-06: Actualizar nivel/peso de habilidad requerida ---
    [HttpPut("{vacanteId}/skills/{skillId}")]
    [Authorize(Roles = "Admin, RRHH")]
    public async Task<IActionResult> UpdateSkillOnVacante(int vacanteId, int skillId, [FromBody] VacanteSkillReqUpdateDTO dto)
    {
        try
        {
            await _vacanteSkillReqService.UpdateSkillOnVacanteAsync(vacanteId, skillId, dto);
            return NoContent(); // 204 No Content (Éxito)
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}