using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IExportacionService
    {
        // --- CAMBIO AQUÍ: Ahora pide ID y Título explícitamente ---
        Task<byte[]> GenerarBrechasExcel(IEnumerable<BrechaSkillDto> brechasData, string tituloVacante);
        Task<byte[]> GenerarKpisExcel(KpiReportDto kpiData, DateTime? desde, DateTime? hasta);
        Task<byte[]> GenerarMatchPdf(MatchResultDTO matchData, ColaboradorDTO? colaboradorData);
        Task<byte[]> GenerarRankingExcel(IEnumerable<MatchResultDTO> rankingData);
        Task EnviarCorreoPersonalizadoAsync(EmailComposeDto correoDto);
    }
}