using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IDashboardService
{
    // Para la vista general (Power BI)
    Task<DashboardCompletoDto> GetDashboardDataAsync();

    // --- NUEVO: Para el endpoint de inventario con filtros ---
    Task<IEnumerable<SkillInventoryDto>> GetInventarioSkillsAsync(int? areaId, int? rolId);
}