using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IColaboradorService
    {
        Task CreateColaboradorAsync(Colaborador colaborador);
        Task DeleteColaboradorAsync(int id);
        Task<IEnumerable<Colaborador>> GetAllColaboradoresAsync();
        Task<Colaborador?> GetColaboradorByIdAsync(int id);
        Task UpdateColaboradorAsync(Colaborador colaborador);
    }
}