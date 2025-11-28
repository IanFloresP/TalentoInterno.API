using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IPostulacionService
    {
        Task<PostulacionDto> CambiarEstadoAsync(int id, string nuevoEstado, string? comentarios);
        Task<PostulacionDto> CrearAsync(CrearPostulacionDto dto);
        Task<IEnumerable<PostulacionDto>> GetPorVacanteAsync(int vacanteId);
    }
}