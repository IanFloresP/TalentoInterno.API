using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IReporteService
{
    ArchivoDTO GenerarMatching(int vacanteId, string formato);
    ArchivoDTO GenerarSkillGaps(string area, string rol, string formato);
    ArchivoDTO GenerarKPIs(DateTime fechaInicio, DateTime fechaFin, string formato);
}