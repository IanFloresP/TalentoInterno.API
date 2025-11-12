using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NivelDominioController : ControllerBase
{
    private readonly INivelDominioService _nivelService;

    public NivelDominioController(INivelDominioService nivelService)
    {
        _nivelService = nivelService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var niveles = await _nivelService.GetAllNivelesDominioAsync();
        var dto = niveles.Select(n => new NivelDominioDto { NivelId = n.NivelId, Nombre = n.Nombre });
        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(byte id)
    {
        var n = await _nivelService.GetNivelDominioByIdAsync(id);
        if (n == null) return NotFound();
        return Ok(new NivelDominioDto { NivelId = n.NivelId, Nombre = n.Nombre });
    }
}
