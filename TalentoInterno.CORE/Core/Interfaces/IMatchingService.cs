using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IMatchingService
    {
        Task<IEnumerable<MatchResultDTO>> GetRankedCandidatesAsync(int vacanteId);
    }
}