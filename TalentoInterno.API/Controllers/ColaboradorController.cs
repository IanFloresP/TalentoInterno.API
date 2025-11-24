using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Services;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin, RRHH, Business Manager")]
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
    [Authorize(Roles = "Admin, RRHH")]
    public async Task<IActionResult> CreateColaborador([FromBody] ColaboradorCreateDTO colaboradorDTO)
    {
        // 1. Crea la entidad
        var nuevoColaborador = await _colaboradorService.CreateColaboradorAsync(colaboradorDTO);

        // 2. (CAMBIO) Busca el DTO limpio para devolverlo en la respuesta
        // Esto oculta el passwordHash y los nulos feos
        var colaboradorResponse = await _colaboradorService.GetColaboradorByIdAsync(nuevoColaborador.ColaboradorId);

        return CreatedAtAction(nameof(GetById), new { id = nuevoColaborador.ColaboradorId }, colaboradorResponse);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, RRHH")]
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