using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Services;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, RRHH")] // Solo RRHH gestiona esto
public class PostulacionController : ControllerBase
{
    private readonly IPostulacionService _postulacionService; // (Necesitas crear este servicio)
    private readonly IVacanteService _vacanteService;

    public PostulacionController(IPostulacionService postulacionService, IVacanteService vacanteService)
    {
        _postulacionService = postulacionService;
        _vacanteService = vacanteService;
    }

    // 1. Iniciar proceso con un candidato (POST)
    [HttpPost]
    public async Task<IActionResult> PostularCandidato([FromBody] CrearPostulacionDto dto)
    {
        var postulacion = await _postulacionService.CrearAsync(dto);
        return Ok(postulacion);
    }

    // 2. Cambiar estado (Ej: De "Entrevista" a "Seleccionado")
    [HttpPut("{id}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoDto dto)
    {
        // Actualizamos la postulación
        var postulacion = await _postulacionService.CambiarEstadoAsync(id, dto.NuevoEstado, dto.Comentarios);

        // --- LÓGICA DE CIERRE AUTOMÁTICO ---
        if (dto.NuevoEstado == "Seleccionado")
        {
            // Si seleccionamos a alguien, cerramos la vacante
            var vacanteDto = new VacanteUpdateDTO
            {
                // Necesitarías obtener los datos actuales de la vacante primero para no borrarlos
                Estado = "Cerrada"
                // ... mapear los otros campos requeridos ...
            };

            // O mejor aún, crear un método específico en IVacanteService: CerrarVacante(id)
            await _vacanteService.CerrarVacanteAsync(postulacion.VacanteId);
        }

        return Ok(postulacion);
    }

    // 3. Ver candidatos en proceso por vacante
    [HttpGet("vacante/{vacanteId}")]
    public async Task<IActionResult> GetPorVacante(int vacanteId)
    {
        var lista = await _postulacionService.GetPorVacanteAsync(vacanteId);
        return Ok(lista);
    }
}