using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Services;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColaboradorController : ControllerBase
{
    private readonly IColaboradorService _colaboradorService;

    public ColaboradorController(IColaboradorService colaboradorService)
    {
        _colaboradorService = colaboradorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetColaboradores()
    {
        var colaboradores = await _colaboradorService.GetAllColaboradoresAsync();
        return Ok(colaboradores);
    }

    [HttpGet("activos")]
    public async Task<IActionResult> GetColaboradoresActivos()
    {
        var colaboradores = await _colaboradorService.GetAllColaboradoresAsync();
        var activos = colaboradores.Where(c => c.Activo == true);
        return Ok(activos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var colaborador = await _colaboradorService.GetColaboradorByIdAsync(id);
        if (colaborador == null)
        {
            return NotFound($"No se encontró un colaborador con el ID {id}.");
        }
        return Ok(colaborador);
    }

    [HttpGet("{id}/rol")]
    public async Task<IActionResult> GetRol(int id)
    {
        var colaborador = await _colaboradorService.GetColaboradorByIdAsync(id);
        if (colaborador == null)
        {
            return NotFound($"No se encontró un colaborador con el ID {id}.");
        }
        var rol = colaborador.RolNombre;
        if (rol == null)
        {
            return NotFound($"No se encontró un rol para el colaborador con el ID {id}.");
        }
        return Ok(rol);
    }

    [HttpPost]
    // ¡CAMBIO AQUÍ! Recibe ColaboradorCreateDTO
    public async Task<IActionResult> CreateColaborador([FromBody] ColaboradorDTO colaboradorDTO)
    {
        // ¡CAMBIO AQUÍ! Llama al nuevo método del servicio
        var nuevoColaborador = await _colaboradorService.CreateColaboradorAsync(colaboradorDTO);

        // Esta respuesta (201 Created) es la correcta y usa el ID del colaborador recién creado.
        return CreatedAtAction(nameof(GetById), new { id = nuevoColaborador.ColaboradorId }, nuevoColaborador);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateColaborador(int id, [FromBody] ColaboradorDTO colaboradorDTO)
    {
        if (id != colaboradorDTO.ColaboradorId)
        {
            return BadRequest("El ID del colaborador no coincide con el ID proporcionado.");
        }

        await _colaboradorService.UpdateColaboradorAsync(colaboradorDTO);
        return NoContent();
    }
}