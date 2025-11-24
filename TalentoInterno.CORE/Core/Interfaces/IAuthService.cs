using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> GetSessionAsync(int usuarioId);
        LoginResponseDTO Login(LoginRequestDTO request);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        void Logout(string token);
    }
}