using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IAuditoriaService
    {
        Task<IEnumerable<AuditoriaGetDto>> ObtenerHistorialAsync(int? usuarioId, DateTime? desde, DateTime? hasta, string? accion);
        Task RegistrarAccionAsync(AuditoriaCreateDto dto);
        Task<IEnumerable<AuditoriaResumenDto>> ObtenerResumenAsync();
    }
}