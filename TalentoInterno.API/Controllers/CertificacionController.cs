using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, RRHH, Business Manager")] // Todos pueden ver
public class CertificacionController : ControllerBase
{
    private readonly ICertificacionService _service;

    public CertificacionController(ICertificacionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, RRHH")] // Solo RRHH/Admin crea
    public async Task<IActionResult> Create([FromBody] CertificacionCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.CertificacionId }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, RRHH")] // Solo RRHH/Admin edita
    public async Task<IActionResult> Update(int id, [FromBody] CertificacionUpdateDto dto)
    {
        try
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Solo Admin borra (por seguridad)
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}