using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> GetSessionAsync(int usuarioId);
    }
}