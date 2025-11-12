using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
}
