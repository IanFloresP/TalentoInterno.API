using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TalentoInterno.CORE.Core.Services;

public class DashboardService : IDashboardService
{
    private readonly TalentoInternooContext _context;

    public DashboardService(TalentoInternooContext context)
    {
        _context = context;
    }

    // --- Método existente (Power BI) ---
    public async Task<DashboardCompletoDto> GetDashboardDataAsync()
    {
        // ... (MANTÉN TU CÓDIGO ACTUAL AQUÍ) ...
        // (El que genera KpisPrincipales, VacantesPorEstado, etc.)

        var response = new DashboardCompletoDto();
        // ... (Lógica que ya tienes) ...
        // (Por brevedad no la repito toda, pero déjala igual)

        // Solo asegúrate de retornar 'response' al final
        return await GenerarDashboardCompletoInterno(); // (O pega tu código aquí)
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
}