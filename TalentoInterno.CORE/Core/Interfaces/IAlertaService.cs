using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IAlertaService
    {
        Task<AlertasResponseDto> GenerarAlertasAsync(string? tipo = null, int? id = null, int? umbral = null);
    }
}