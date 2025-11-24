using Microsoft.Extensions.Logging;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class ReporteService : IReporteService
{
    private readonly ILogger<ReporteService> _logger;

    public ReporteService(ILogger<ReporteService> logger)
    {
        _logger = logger;
    }

    public ArchivoDTO GenerarMatching(int vacanteId, string formato)
    {
        _logger.LogInformation("Generando reporte de matching para vacante {VacanteId} en formato {Formato}", vacanteId, formato);

        return new ArchivoDTO
        {
            ContentType = "application/pdf",
            FileName = $"Matching_Vacante_{vacanteId}.{formato}",
            Data = new byte[0]
        };
    }

    public ArchivoDTO GenerarSkillGaps(string area, string rol, string formato)
    {
        _logger.LogInformation("Generando reporte de brechas de skills para \u00e1rea {Area}, rol {Rol}, formato {Formato}", area, rol, formato);

        return new ArchivoDTO
        {
            ContentType = "application/vnd.ms-excel",
            FileName = $"SkillGaps_{area}_{rol}.{formato}",
            Data = new byte[0]
        };
    }

    public ArchivoDTO GenerarKPIs(DateTime fechaInicio, DateTime fechaFin, string formato)
    {
        _logger.LogInformation("Generando reporte de KPIs desde {FechaInicio} hasta {FechaFin} en formato {Formato}", fechaInicio, fechaFin, formato);

        return new ArchivoDTO
        {
            ContentType = "application/pdf",
            FileName = $"KPIs_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.{formato}",
            Data = new byte[0]
        };
    }
}