using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface ICertificacionService
    {
        Task<CertificacionDto> CreateAsync(CertificacionCreateDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<CertificacionDto>> GetAllAsync();
        Task<CertificacionDto?> GetByIdAsync(int id);
        Task UpdateAsync(int id, CertificacionUpdateDto dto);
    }
}