using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using System.Threading.Tasks; // Asegúrate de tener los usings
using System.Linq; // Para .Select()

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolController : ControllerBase
{
    private readonly IRolService _rolService;

    public RolController(IRolService rolService)
    {
        _rolService = rolService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _rolService.GetAllRolesAsync();
        var dto = roles.Select(r => new RolDto { RolId = r.RolId, Nombre = r.Nombre });
        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var rol = await _rolService.GetRoleByIdAsync(id);
        if (rol == null) return NotFound();
        return Ok(new RolDto { RolId = rol.RolId, Nombre = rol.Nombre });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RolDto rolDto)
    {
        var rol = new Rol { Nombre = rolDto.Nombre };
        await _rolService.CreateRoleAsync(rol);
        // Devolvemos el DTO con el ID asignado
        var dto = new RolDto { RolId = rol.RolId, Nombre = rol.Nombre };
        return CreatedAtAction(nameof(GetById), new { id = rol.RolId }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RolDto rolDto)
    {
        var existing = await _rolService.GetRoleByIdAsync(id);
        if (existing == null) return NotFound();

        existing.Nombre = rolDto.Nombre;
        await _rolService.UpdateRoleAsync(existing);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _rolService.GetRoleByIdAsync(id);
        if (existing == null) return NotFound();

        await _rolService.DeleteRoleAsync(id);
        return NoContent();
    }
}