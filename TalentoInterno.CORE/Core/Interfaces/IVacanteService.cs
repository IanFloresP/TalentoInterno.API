using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IVacanteService
    {
        Task<Vacante> CreateVacanteAsync(VacanteCreateDTO dto);
        Task DeleteVacanteAsync(int id);
        Task<IEnumerable<VacanteListDTO>> GetAllVacantesAsync();
        Task<VacanteGetDTO?> GetVacanteByIdAsync(int id);
        Task UpdateVacanteAsync(int id, VacanteUpdateDTO dto);
    }
}