using Microsoft.Extensions.Logging;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class DashboardService : IDashboardService
{
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(ILogger<DashboardService> logger)
    {
        _logger = logger;
    }

    public InventarioSkillsDTO ObtenerInventarioSkills(string area, string rol)
    {
        _logger.LogInformation("Consultando inventario de skills para área {Area}, rol {Rol}", area, rol);

        return new InventarioSkillsDTO
        {
            Area = area,
            Rol = rol,
            Skills = new List<string> { "Python", "SQL", "Machine Learning" },
            TotalesPorSkill = new System.Collections.Generic.Dictionary<string, int> { { "Python", 10 }, { "SQL", 8 } },
            Brechas = new List<string> { "Deep Learning" }
        };
    }

    public KPIsDTO ObtenerKPIs(DateTime fechaInicio, DateTime fechaFin)
    {
        _logger.LogInformation("Consultando KPIs desde {FechaInicio} hasta {FechaFin}", fechaInicio, fechaFin);

        return new KPIsDTO
        {
            VacantesTotales = 20,
            VacantesCubiertasInternas = 12,
            TiempoPromedioCobertura = 15.5,
            SkillsCriticos = new List<string> { "Cloud Computing", "Data Engineering" },
            Tendencias = new System.Collections.Generic.Dictionary<string, double> { { "Python", 0.8 }, { "SQL", 0.6 } }
        };
    }
}