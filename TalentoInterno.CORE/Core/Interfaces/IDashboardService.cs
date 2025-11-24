using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IDashboardService
{
    InventarioSkillsDTO ObtenerInventarioSkills(string area, string rol);
    KPIsDTO ObtenerKPIs(DateTime fechaInicio, DateTime fechaFin);
}