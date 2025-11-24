using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Services;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/ColaboradorCertificacion")]
[Authorize(Roles = "Admin, RRHH, Business Manager")]
public class ColaboradorCertificacionController : ControllerBase
{
    private readonly IColaboradorCertificacionService _service;

    public ColaboradorCertificacionController(IColaboradorCertificacionService service)
    {
        _service = service;
    }

    [HttpGet("{colaboradorId}")]
    public async Task<IActionResult> Get(int colaboradorId)
    {
        var certs = await _service.GetCertificacionesAsync(colaboradorId);
        return Ok(certs);
    }

    [HttpPost("{colaboradorId}")]
    [Authorize(Roles = "Admin, RRHH")] // Solo ellos pueden agregar
    public async Task<IActionResult> Add(int colaboradorId, [FromBody] ColaboradorCertificacionCreateDto dto)
    {
        await _service.AddCertificacionAsync(colaboradorId, dto);
        return Ok(new { message = "Certificación agregada exitosamente." });
    }

    [HttpDelete("{colaboradorId}/{certificacionId}")]
    [Authorize(Roles = "Admin, RRHH")]
    public async Task<IActionResult> Delete(int colaboradorId, int certificacionId)
    {
        await _service.RemoveCertificacionAsync(colaboradorId, certificacionId);
        return NoContent();
    }
}