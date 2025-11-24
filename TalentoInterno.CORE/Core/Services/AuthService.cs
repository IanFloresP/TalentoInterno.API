using Microsoft.Extensions.Logging;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Services;

public class AuthService : IAuthService
{
    private readonly IJWTService _jwtService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IJWTService jwtService, ILogger<AuthService> logger)
    {
        _jwtService = jwtService;
        _logger = logger;
    }

    public LoginResponseDTO Login(LoginRequestDTO request)
    {
        _logger.LogInformation("Intento de login para usuario {Email}", request.Email);

        // Simulación: en implementación real se valida hash/salt
        var colaboradorId = 1; // Ejemplo
        var role = "User";

        var token = _jwtService.GenerateJWToken(new Colaborador
        {
            ColaboradorId = colaboradorId,
            Nombres = "Demo",
            Apellidos = "User",
            Email = request.Email,
            Rol = new Rol { Nombre = role }
        });

        return new LoginResponseDTO
        {
            AccessToken = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            Role = role,
            ColaboradorId = colaboradorId,
            RefreshToken = System.Guid.NewGuid().ToString()
        };
    }

    public void Logout(string token)
    {
        _logger.LogInformation("Logout ejecutado para token {Token}", token);
    }

    public bool Validate(string token)
    {
        _logger.LogInformation("Validando token {Token}", token);
        return true; // Simulación
    }

    public LoginResponseDTO Refresh(string refreshToken)
    {
        _logger.LogInformation("Generando nuevo token a partir de refresh token {RefreshToken}", refreshToken);

        return new LoginResponseDTO
        {
            AccessToken = "nuevoTokenJWT",
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            Role = "User",
            ColaboradorId = 1,
            RefreshToken = System.Guid.NewGuid().ToString()
        };
    }
}