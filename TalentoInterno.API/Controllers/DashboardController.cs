using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/dashboard")] // Ruta base
[Authorize(Roles = "Admin, RRHH, Business Manager")] // Accesible para líderes
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly IKpiService _kpiService; // Usamos este porque ya tiene la lógica de fechas

    public DashboardController(IDashboardService dashboardService, IKpiService kpiService)
    {
        _dashboardService = dashboardService;
        _kpiService = kpiService;
    }

    // 1. GET /dashboard/inventario-skills?area=...&rol=...
    // HU-11: Inventario de skills por área y rol
    [HttpGet("inventario-skills")]
    public async Task<IActionResult> GetInventarioSkills([FromQuery] int? area, [FromQuery] int? rol)
    {
        // Nota: Usamos los IDs (area, rol) porque es cómo funciona el backend.
        // El frontend enviará los IDs seleccionados en los dropdowns.
        var inventario = await _dashboardService.GetInventarioSkillsAsync(area, rol);
        return Ok(inventario);
    }

    // 2. GET /dashboard/kpis?desde=...&hasta=...
    // HU-12: KPIs de gestión filtrados por rango de fechas
    [HttpGet("kpis")]
    public async Task<IActionResult> GetKpis([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        // Reutilizamos la lógica potente de KpiService que ya filtra por fechas
        var kpis = await _kpiService.GetKpiDataAsync(desde, hasta);
        return Ok(kpis);
    }

    // 3. GET /dashboard/skills/resumen?area=...
    // HU-12: Resumen de skills por área (distribución y brechas)
    [HttpGet("skills/resumen")]
    public async Task<IActionResult> GetResumenSkills([FromQuery] int? area)
    {
        // Para el resumen, obtenemos el inventario filtrado solo por el área solicitada.
        // Esto permite ver la "oferta" de talento de esa área específica.
        var resumen = await _dashboardService.GetInventarioSkillsAsync(area, null);
        return Ok(resumen);
    }
}