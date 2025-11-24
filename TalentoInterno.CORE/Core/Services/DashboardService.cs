using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;

namespace TalentoInterno.CORE.Core.Services;

public class DashboardService : IDashboardService
{
    private readonly TalentoInternooContext _context;
    private readonly ILogger<DashboardService> _logger;
    public DashboardService(TalentoInternooContext context, ILogger<DashboardService> logger)
    {
        _context = context;
        _logger = logger;
    }


    // Método auxiliar para no borrar tu código (Pega tu lógica aquí o arriba)
    private async Task<DashboardCompletoDto> GenerarDashboardCompletoInterno()
    {
        var response = new DashboardCompletoDto();

        // 1. KPIs
        var totalColab = await _context.Colaborador.CountAsync(c => c.Activo == true);
        var vacantesAbiertas = await _context.Vacante.CountAsync(v => v.Estado == "Abierta");
        var vacantesCriticas = await _context.Vacante.CountAsync(v => v.Estado == "Abierta" && v.UrgenciaId == 3);

        response.KpisPrincipales = new List<KpiCardDto>
        {
            new KpiCardDto { Titulo = "Colaboradores Activos", Valor = totalColab, Icono = "people", Color = "blue" },
            new KpiCardDto { Titulo = "Vacantes Abiertas", Valor = vacantesAbiertas, Icono = "work", Color = "green" },
            new KpiCardDto { Titulo = "Vacantes Críticas", Valor = vacantesCriticas, Icono = "warning", Color = "red" }
        };

        // ... (Resto de tu lógica de gráficos que me mostraste) ...
        // Gráfico Dona, Barras, etc.
        // ...

        return response;
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

    // --- NUEVO MÉTODO: INVENTARIO CON FILTROS (HU-11) ---
    public async Task<IEnumerable<SkillInventoryDto>> GetInventarioSkillsAsync(int? areaId, int? rolId)
    {
        var query = _context.ColaboradorSkill
            .Include(cs => cs.Colaborador).ThenInclude(c => c.Area)
            .Include(cs => cs.Skill)
            .Include(cs => cs.Nivel)
            .Where(cs => cs.Colaborador.Activo == true) // Solo gente activa
            .AsQueryable();

        // 1. Aplicar Filtros
        if (areaId.HasValue)
            query = query.Where(cs => cs.Colaborador.AreaId == areaId.Value);

        if (rolId.HasValue)
            query = query.Where(cs => cs.Colaborador.RolId == rolId.Value);

        // 2. Agrupar y Proyectar
        var inventario = await query
            .GroupBy(cs => new { Skill = cs.Skill.Nombre, Nivel = cs.Nivel.Nombre, Area = cs.Colaborador.Area.Nombre })
            .Select(g => new SkillInventoryDto
            {
                SkillNombre = g.Key.Skill,
                NivelNombre = g.Key.Nivel,
                AreaNombre = g.Key.Area ?? "Sin Área",
                CantidadPersonas = g.Count()
            })
            .OrderBy(x => x.SkillNombre)
            .ToListAsync();

        return inventario;
    }
    public InventarioSkillsDTO ObtenerInventarioSkills(string area, string rol)
    {
        _logger.LogInformation("Consultando inventario de skills para Área {Area}, rol {Rol}", area, rol);
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

    // --- Método existente (Power BI) ---


    public async Task<DashboardCompletoDto> GetDashboardDataAsync()
    {
        var response = new DashboardCompletoDto();

        // 1. KPIs Principales
        var totalColab = await _context.Colaborador.CountAsync(c => c.Activo == true);
        var vacantesAbiertas = await _context.Vacante.CountAsync(v => v.Estado == "Abierta");
        var vacantesCriticas = await _context.Vacante.CountAsync(v => v.Estado == "Abierta" && v.UrgenciaId == 3);

        response.KpisPrincipales = new List<KpiCardDto>
    {
        new KpiCardDto { Titulo = "Colaboradores", Valor = totalColab, Icono = "people", Color = "blue" },
        new KpiCardDto { Titulo = "Vacantes", Valor = vacantesAbiertas, Icono = "work", Color = "green" },
        new KpiCardDto { Titulo = "Críticas", Valor = vacantesCriticas, Icono = "warning", Color = "red" }
    };

        // 2. Gráfico Dona (Vacantes por Estado)
        var vacantesPorEstado = await _context.Vacante
            .GroupBy(v => v.Estado)
            .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
            .ToListAsync();

        response.VacantesPorEstado.Etiquetas = vacantesPorEstado.Select(x => x.Estado).ToList();
        response.VacantesPorEstado.Valores = vacantesPorEstado.Select(x => x.Cantidad).ToList();

        // 3. Gráfico Barras (Top Skills)
        var topSkills = await _context.VacanteSkillReq
            .Where(r => r.Vacante.Estado == "Abierta")
            .GroupBy(r => r.Skill.Nombre)
            .Select(g => new { Skill = g.Key, Demanda = g.Count() })
            .OrderByDescending(x => x.Demanda)
            .Take(5)
            .ToListAsync();

        response.TopSkillsDemandadas.Etiquetas = topSkills.Select(x => x.Skill).ToList();
        response.TopSkillsDemandadas.Valores = topSkills.Select(x => x.Demanda).ToList();

        // 4. Gráfico Apilado (Categorías y Series)
        var targetSkills = response.TopSkillsDemandadas.Etiquetas;
        var dataInventario = await _context.ColaboradorSkill
            .Where(cs => targetSkills.Contains(cs.Skill.Nombre))
            .Include(cs => cs.Skill).Include(cs => cs.Nivel)
            .ToListAsync();

        response.InventarioTalento.Categorias = targetSkills; // Eje X

        var niveles = await _context.NivelDominio.OrderBy(n => n.NivelId).ToListAsync();
        foreach (var nivel in niveles)
        {
            var serie = new SerieDatosDto { Nombre = nivel.Nombre }; // Ej: "Avanzado"
            foreach (var skillName in targetSkills)
            {
                int count = dataInventario.Count(x => x.Skill.Nombre == skillName && x.NivelId == nivel.NivelId);
                serie.Datos.Add(count);
            }
            response.InventarioTalento.Series.Add(serie);
        }

        return response;
    }
}