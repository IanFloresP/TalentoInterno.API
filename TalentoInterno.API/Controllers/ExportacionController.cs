using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TalentoInterno.API.Filters;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Services;
using System.IO; // Necesario para limpiar nombre de archivo
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/exportacion")]
[Authorize(Roles = "Admin, RRHH, Business Manager")]
[ServiceFilter(typeof(AuditoriaFilter))]
public class ExportacionController : ControllerBase
{
    private readonly IExportacionService _exportacionService;
    private readonly IMatchingService _matchingService;
    private readonly IColaboradorSkillService _colabSkillService;
    private readonly IColaboradorService _colaboradorService;
    private readonly IKpiService _kpiService;
    private readonly IVacanteService _vacanteService; // --- 1. NUEVA INYECCIÓN ---
    private readonly IAreaService _areaService; // <--- 1. NUEVA INYECCIÓN

    public ExportacionController(
        IExportacionService exportacionService,
        IMatchingService matchingService,
        IColaboradorSkillService colabSkillService,
        IColaboradorService colaboradorService,
        IKpiService kpiService,
        IVacanteService vacanteService,
        IAreaService areaService) // <--- 1. NUEVA INYECCIÓN
    {
        _exportacionService = exportacionService;
        _matchingService = matchingService;
        _colabSkillService = colabSkillService;
        _colaboradorService = colaboradorService;
        _kpiService = kpiService;
        _vacanteService = vacanteService;
        _areaService = areaService;
    }


    [HttpGet("{vacanteId}/pdf")]
    public async Task<IActionResult> ExportarBookPdf(int vacanteId)
    {
        // 1. Obtener datos y filtrar los Admins si es necesario 
        // (Tu servicio ya debería traer solo colaboradores activos y no admins, pero por si acaso)
        var ranking = await _matchingService.GetRankedCandidatesAsync(vacanteId);

        // Filtro extra de seguridad: Remover Rol "Admin" si se coló alguno
        var rankingFiltrado = ranking.Where(c => c.RolNombre != "Admin").ToList();

        var vacante = await _vacanteService.GetVacanteByIdAsync(vacanteId);
        string titulo = vacante?.Titulo ?? "Vacante";

        // 2. Generar el PDF
        var fileBytes = await _exportacionService.GenerarPdfConsolidadoAsync(rankingFiltrado, titulo);

        return File(fileBytes, "application/pdf", $"Book_Candidatos_{DateTime.Now:yyyyMMdd}.pdf");
    }

    [HttpGet("kpis")]
    public async Task<IActionResult> ExportarKPIs([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        var data = await _kpiService.GetKpiDataAsync(desde, hasta);
        byte[] fileBytes = await _exportacionService.GenerarKpisExcel(data, desde, hasta);

        if (fileBytes == null || fileBytes.Length == 0)
            return StatusCode(500, "No se generó el archivo. Revise los datos de entrada.");

        var ms = new System.IO.MemoryStream(fileBytes);
        ms.Position = 0;
        return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_KPIs.xlsx");
    }
    // 2. ENDPOINT EXCEL (El reporte consolidado inteligente)
    [HttpGet("{vacanteId}/brechas")]
    public async Task<IActionResult> DescargarReporteConsolidado(int vacanteId)
    {
        // a) Obtenemos los datos calculados
        var ranking = await _matchingService.GetRankedCandidatesAsync(vacanteId);

        // b) Obtenemos el nombre de la vacante para el título del Excel
        var vacante = await _vacanteService.GetVacanteByIdAsync(vacanteId);
        string titulo = vacante != null ? vacante.Titulo : "Desconocida";

        // c) Generamos el archivo usando tu nuevo método "Sábana de Datos"
        var archivoExcel = await _exportacionService.GenerarReporteConsolidadoAsync(ranking, titulo);

        // d) Devolvemos el archivo al navegador
        string nombreArchivo = $"Reporte_Talento_{titulo}_{DateTime.Now:yyyyMMdd}.xlsx";
        return File(archivoExcel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
    }

}
