using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardCompletoDto> GetDashboardDataAsync();
        Task<IEnumerable<SkillInventoryDto>> GetInventarioSkillsAsync(int? areaId, int? rolId);
        InventarioSkillsDTO ObtenerInventarioSkills(string area, string rol);
        KPIsDTO ObtenerKPIs(DateTime fechaInicio, DateTime fechaFin);
    }
}