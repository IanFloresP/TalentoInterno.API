using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/exportacion")]
public class ExportacionController : ControllerBase
{
    private readonly IExportacionService _exportacionService;
    private readonly IMatchingService _matchingService;
    private readonly IColaboradorSkillService _colabSkillService;
    private readonly IColaboradorService _colaboradorService;
    

    public ExportacionController(
        IExportacionService exportacionService,
        IMatchingService matchingService,
        IColaboradorSkillService colabSkillService,
        IColaboradorService colaboradorService
        )
    {
        _exportacionService = exportacionService;
        _matchingService = matchingService;
        _colabSkillService = colabSkillService;
        _colaboradorService = colaboradorService;
        
    }

    [HttpGet("ranking/{vacanteId}")]
    public async Task<IActionResult> ExportarRanking(int vacanteId)
    {
        var data = await _matchingService.GetRankedCandidatesAsync(vacanteId);
        byte[] fileBytes = await _exportacionService.GenerarRankingExcel(data);
        string fileName = $"Ranking_Vacante_{vacanteId}.xlsx";
        string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        return File(fileBytes, mimeType, fileName);
    }

    [HttpGet("match/{colaboradorId}/{vacanteId}")]
    public async Task<IActionResult> ExportarMatch(int colaboradorId, int vacanteId)
    {
        var matchData = await _colabSkillService.GetMatchDetailsAsync(colaboradorId, vacanteId);
        if (matchData == null) return NotFound("Datos del match no encontrados.");

        var colaboradorData = await _colaboradorService.GetColaboradorByIdAsync(colaboradorId);
        if (colaboradorData == null) return NotFound("Colaborador no encontrado.");

        byte[] fileBytes = await _exportacionService.GenerarMatchPdf(matchData, colaboradorData);

        string fileName = $"Match_{colaboradorData.Nombres}_Vac{vacanteId}.pdf";
        string mimeType = "application/pdf";
        return File(fileBytes, mimeType, fileName);
    }

    /*[HttpGet("kpis")]
    
    public async Task<IActionResult> ExportarKPIs()
    {
        var data = await _kpiService.GetKpiDataAsync();
        byte[] fileBytes = await _exportacionService.GenerarKpisExcel(data);
        string fileName = "Reporte_KPIs_Vacantes.xlsx";
        string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        return File(fileBytes, mimeType, fileName);
    }
    */

    [HttpGet("brechas")]
    public async Task<IActionResult> ExportarBrechas([FromQuery] int vacanteId)
    {
        var data = await _colabSkillService.GetSkillGapsForVacanteAsync(vacanteId, null);
        byte[] fileBytes = await _exportacionService.GenerarBrechasExcel(data);
        string fileName = $"Brechas_Vacante_{vacanteId}.xlsx";
        string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        return File(fileBytes, mimeType, fileName);
    }
}