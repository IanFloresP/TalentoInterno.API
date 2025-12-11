using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComunicacionController : ControllerBase
{
    private readonly IExportacionService _emailService;

    public ComunicacionController(IExportacionService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("redactar")]
    // [Authorize] <--- Descomentar si quieres que solo usuarios logueados envíen
    public async Task<IActionResult> RedactarCorreo([FromForm] EmailComposeDto dto)
    {
        try
        {
            // Validaciones extra si quieres (ej: peso máximo de archivos)
            if (dto.Adjuntos != null)
            {
                long size = dto.Adjuntos.Sum(f => f.Length);
                if (size > 10 * 1024 * 1024) // 10 MB límite
                    return BadRequest("El total de archivos no puede superar los 10MB");
            }

            await _emailService.EnviarCorreoPersonalizadoAsync(dto);
            return Ok(new { message = "Correo enviado exitosamente con sus adjuntos." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error enviando correo: " + ex.Message });
        }
    }
}