using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Services
{
    public interface IColaboradorService
    {
        Task<Colaborador> CreateColaboradorAsync(ColaboradorCreateDTO colaboradorDTO);
        Task DeleteColaboradorAsync(int id);
        Task<IEnumerable<ColaboradorDTO>> GetAllColaboradoresAsync();
        Task<ColaboradorDTO?> GetColaboradorByIdAsync(int id);
        Task UpdateColaboradorAsync(ColaboradorDTO dto);
    }
}