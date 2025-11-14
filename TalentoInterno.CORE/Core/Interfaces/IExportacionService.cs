using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IExportacionService
    {
        Task<byte[]> GenerarBrechasExcel(IEnumerable<object> brechasData);
        Task<byte[]> GenerarMatchPdf(MatchResultDTO matchData, ColaboradorDTO? colaboradorData);
        Task<byte[]> GenerarRankingExcel(IEnumerable<MatchResultDTO> rankingData);
    }
}