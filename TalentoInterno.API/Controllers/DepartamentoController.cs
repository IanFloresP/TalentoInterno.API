using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, RRHH, Business Manager")]
public class DepartamentoController : ControllerBase
{
    private readonly IDepartamentoService _departamentoService;

    public DepartamentoController(IDepartamentoService departamentoService)
    {
        _departamentoService = departamentoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var departamentos = await _departamentoService.GetAllDepartamentosAsync();
        var dto = departamentos.Select(d => new DepartamentoDto { DepartamentoId = d.DepartamentoId, Nombre = d.Nombre });
        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var d = await _departamentoService.GetDepartamentoByIdAsync(id);
        if (d == null) return NotFound();
        return Ok(new DepartamentoDto { DepartamentoId = d.DepartamentoId, Nombre = d.Nombre });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DepartamentoDto departamentoDto)
    {
        var departamento = new Departamento { Nombre = departamentoDto.Nombre };
        await _departamentoService.CreateDepartamentoAsync(departamento);
        return CreatedAtAction(nameof(GetById), new { id = departamento.DepartamentoId }, new DepartamentoDto { DepartamentoId = departamento.DepartamentoId, Nombre = departamento.Nombre });
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DepartamentoDto departamentoDto)
    {
        if (id != departamentoDto.DepartamentoId)
            return BadRequest("El id de la ruta debe coincidir con DepartamentoId.");
        var departamento = new Departamento { DepartamentoId = departamentoDto.DepartamentoId, Nombre = departamentoDto.Nombre };
        await _departamentoService.UpdateDepartamentoAsync(departamento);
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _departamentoService.DeleteDepartamentoAsync(id);
        // No es necesario asignar el resultado a una variable, ya que el método es void.
        // Si necesitas saber si se eliminó correctamente, cambia la interfaz para devolver un bool.
        return NoContent();
    }
}
