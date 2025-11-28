using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TalentoInterno.API.Filters;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, RRHH")] // Solo gestión
[ServiceFilter(typeof(AuditoriaFilter))] // Auditoría obligatoria
public class PostulacionController : ControllerBase
{
    private readonly IPostulacionService _postulacionService;

    public PostulacionController(IPostulacionService postulacionService)
    {
        _postulacionService = postulacionService;
    }

    // 1. Postulación Masiva (Seleccionar varios del ranking)
    // POST: api/Postulacion/masivo
    [HttpPost("masivo")]
    public async Task<IActionResult> PostularMasivo([FromBody] CrearPostulacionMasivaDto dto)
    {
        try
        {
            var resultados = await _postulacionService.CrearMasivoAsync(dto);
            return Ok(resultados);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 2. Ver candidatos en proceso
    // GET: api/Postulacion/vacante/1
    [HttpGet("vacante/{vacanteId}")]
    public async Task<IActionResult> GetPorVacante(int vacanteId)
    {
        var lista = await _postulacionService.GetPorVacanteAsync(vacanteId);
        return Ok(lista);
    }

    // 3. Avanzar en el proceso (Entrevista, Seleccionado)
    // PUT: api/Postulacion/5/estado
    [HttpPut("{id}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoDto dto)
    {
        try
        {
            var resultado = await _postulacionService.CambiarEstadoAsync(id, dto);
            return Ok(resultado);
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    // 4. Rechazar candidato
    // PUT: api/Postulacion/5/rechazar
    [HttpPut("{id}/rechazar")]
    public async Task<IActionResult> Rechazar(int id, [FromBody] RechazarCandidatoDto dto)
    {
        try
        {
            var resultado = await _postulacionService.RechazarAsync(id, dto.MotivoRechazo);
            return Ok(resultado);
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }
}