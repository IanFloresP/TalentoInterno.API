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

    // AHORA RECIBE UNA LISTA
    [HttpPost("{colaboradorId}/masivo")]
    [Authorize(Roles = "Admin, RRHH")]
    public async Task<IActionResult> AddMany(
        int colaboradorId,
        [FromBody] List<ColaboradorCertificacionCreateDto> dtos)
    {
        if (dtos == null || dtos.Count == 0)
            return BadRequest(new { message = "No se enviaron certificaciones." });

        foreach (var dto in dtos)
        {
            await _service.AddCertificacionAsync(colaboradorId, dto);
        }

        return Ok(new { message = "Certificaciones agregadas exitosamente." });
    }

    [HttpDelete("{colaboradorId}/{certificacionId}")]
    [Authorize(Roles = "Admin, RRHH")]
    public async Task<IActionResult> Delete(int colaboradorId, int certificacionId)
    {
        await _service.RemoveCertificacionAsync(colaboradorId, certificacionId);
        return NoContent();
    }
}