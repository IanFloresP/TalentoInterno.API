using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IAuthService
{
    LoginResponseDTO Login(LoginRequestDTO request);
    void Logout(string token);
    bool Validate(string token);
    LoginResponseDTO Refresh(string refreshToken);
}