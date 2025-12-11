using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Services;

namespace TalentoInterno.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, RRHH, Business Manager")] // Seguridad aplicada
public class MatchingController : ControllerBase
{
    private readonly IMatchingService _matchingService;
    private readonly IExportacionService _exportacionService;
    private readonly IVacanteService _vacanteService; // Para sacar el título de la vacante

    // Inyectamos los servicios que ya creaste (DIP - Dependency Inversion)
    public MatchingController(
        IMatchingService matchingService,
        IExportacionService exportacionService,
        IVacanteService vacanteService)
    {
        _matchingService = matchingService;
        _exportacionService = exportacionService;
        _vacanteService = vacanteService;
    }

    // 1. ENDPOINT JSON (Para ver el ranking en pantalla/frontend)
    [HttpGet("{vacanteId}")]
    public async Task<IActionResult> GetRanking(int vacanteId)
    {
        // Llamamos a tu servicio nuevo que ya tiene la lógica de puntos, filtros y DTOs completos
        var ranking = await _matchingService.GetRankedCandidatesAsync(vacanteId);
        return Ok(ranking);
    }

    
}