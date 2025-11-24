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

    [HttpGet("ranking/{vacanteId}")]
    public async Task<IActionResult> ExportarRanking(int vacanteId)
    {
        var data = await _matchingService.GetRankedCandidatesAsync(vacanteId);
        byte[] fileBytes = await _exportacionService.GenerarRankingExcel(data);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Ranking_Vacante_{vacanteId}.xlsx");
    }

    [HttpGet("match/{colaboradorId}/{vacanteId}")]
    public async Task<IActionResult> ExportarMatch(int colaboradorId, int vacanteId)
    {
        var matchData = await _colabSkillService.GetMatchDetailsAsync(colaboradorId, vacanteId);
        if (matchData == null) return NotFound("Datos del match no encontrados.");

        var colaboradorEntidad = await _colaboradorService.GetColaboradorByIdAsync(colaboradorId);
        if (colaboradorEntidad == null) return NotFound("Colaborador no encontrado.");

        // Mapeo directo porque el servicio ya devuelve un DTO (si usaste la corrección de ColaboradorService)
        // Si tu servicio devolvía Entidad, aquí mapeas. Asumimos que devuelve DTO:
        var colaboradorDto = colaboradorEntidad;

        byte[] fileBytes = await _exportacionService.GenerarMatchPdf(matchData, colaboradorDto);
        return File(fileBytes, "application/pdf", $"Match_{colaboradorDto.Nombres}_Vac{vacanteId}.pdf");
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

    // GET: api/exportacion/brechas?vacanteId=1&areaId=2
    [HttpGet("brechas")]
    public async Task<IActionResult> ExportarBrechas([FromQuery] int vacanteId, [FromQuery] int? areaId)
    {
        // 1. Obtener los datos filtrados (el servicio ya maneja el filtro de área si se lo pasas)
        var data = await _colabSkillService.GetSkillGapsForVacanteAsync(vacanteId, areaId);

        // 2. Obtener información base para los nombres
        var vacante = await _vacanteService.GetVacanteByIdAsync(vacanteId);
        string nombreVacante = vacante?.Titulo ?? "Vacante";

        // --- LÓGICA DE NOMBRES DINÁMICA ---

        // Caso Base: Solo Vacante
        string tituloReporte = nombreVacante;
        string nombreArchivo = $"Brechas_{nombreVacante}";

        // Caso con Filtro: Vacante + Área
        if (areaId.HasValue)
        {
            // Buscamos el nombre del área (si inyectaste IAreaService, úsalo. Si no, usa el ID)
            // var area = await _areaService.GetByIdAsync(areaId.Value);
            // string nombreArea = area?.Nombre ?? "AreaDesconocida";

            // Simulación si no tienes el servicio de área a mano:
            string nombreArea = $"Area_{areaId}";

            tituloReporte += $" - (Filtrado por: {nombreArea})";
            nombreArchivo += $"_{nombreArea}";
        }

        nombreArchivo += ".xlsx"; // Agregar extensión

        // Limpieza de caracteres prohibidos en nombre de archivo
        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
        {
            nombreArchivo = nombreArchivo.Replace(c, '_');
        }

        // 3. Generar Excel (Llamamos a TU servicio tal cual lo tienes, con 2 argumentos)
        byte[] fileBytes = await _exportacionService.GenerarBrechasExcel(data, tituloReporte);

        // 4. Devolver archivo con el nombre calculado
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
    }
}
