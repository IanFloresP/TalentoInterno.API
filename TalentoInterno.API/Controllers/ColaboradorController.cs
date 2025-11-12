using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;

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
        // El método correcto según la interfaz es GetColaboradorByIdAsync
        var colaborador = await _colaboradorService.GetColaboradorByIdAsync(id);
        if (colaborador == null)
        {
            return NotFound($"No se encontró un colaborador con el ID {id}.");
        }
        // Suponiendo que la propiedad Rol existe en el modelo Colaborador
        var rol = colaborador.Rol;
        if (rol == null)
        {
            return NotFound($"No se encontró un rol para el colaborador con el ID {id}.");
        }
        return Ok(rol);
    }
}