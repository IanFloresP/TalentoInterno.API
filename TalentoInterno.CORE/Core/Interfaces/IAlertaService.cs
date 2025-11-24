using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IAlertaService
{
    // Ahora acepta parámetros opcionales para filtrar
    Task<AlertasResponseDto> GenerarAlertasAsync(string? tipo = null, int? id = null, int? umbral = null);
}