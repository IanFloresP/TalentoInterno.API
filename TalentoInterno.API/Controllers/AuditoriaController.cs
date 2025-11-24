using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/auditoria")]
public class AuditoriaController : ControllerBase
{
    private readonly IAuditoriaService _auditoriaService;

    public AuditoriaController(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    // POST /auditoria → Registrar una acción
    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] AuditoriaCreateDto dto)
    {
        await _auditoriaService.RegistrarAccionAsync(dto);
        return Ok(new { message = "Acción registrada." });
    }

    // GET /auditoria (Con filtros opcionales)
    [HttpGet]
    public async Task<IActionResult> GetHistorial(
        [FromQuery] int? usuarioId,
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta,
        [FromQuery] string? accion)
    {
        var historial = await _auditoriaService.ObtenerHistorialAsync(usuarioId, desde, hasta, accion);
        return Ok(historial);
    }

    // GET /auditoria/resumen
    [HttpGet("resumen")]
    public async Task<IActionResult> GetResumen()
    {
        var resumen = await _auditoriaService.ObtenerResumenAsync();
        return Ok(resumen);
    }
}