using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IKpiService
    {
        Task<KpiReportDto> GetKpiDataAsync(DateTime? desde = null, DateTime? hasta = null);
    }
}