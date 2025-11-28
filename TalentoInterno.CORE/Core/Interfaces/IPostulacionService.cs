using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IPostulacionService
{
    Task<IEnumerable<PostulacionDto>> GetPorVacanteAsync(int vacanteId);

    // Método masivo inteligente
    Task<IEnumerable<PostulacionDto>> CrearMasivoAsync(CrearPostulacionMasivaDto dto);

    // Métodos de flujo
    Task<PostulacionDto> CambiarEstadoAsync(int postulacionId, CambiarEstadoDto dto);
    Task<PostulacionDto> RechazarAsync(int postulacionId, string motivo);
}