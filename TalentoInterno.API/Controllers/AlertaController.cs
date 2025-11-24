using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")] // Ruta: api/Alerta
[Authorize(Roles = "Admin, RRHH, Business Manager")]
public class AlertaController : ControllerBase
{
    private readonly IAlertaService _alertaService;

    public AlertaController(IAlertaService alertaService)
    {
        _alertaService = alertaService;
    }

    // GET /api/Alerta?tipo=vacante&id=1&umbral=60
    [HttpGet]
    public async Task<IActionResult> GetAlertas(
        [FromQuery] string? tipo,
        [FromQuery] int? id,
        [FromQuery] int? umbral)
    {
        var alertas = await _alertaService.GenerarAlertasAsync(tipo, id, umbral);
        return Ok(alertas);
    }
}