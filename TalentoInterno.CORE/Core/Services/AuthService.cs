using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;
namespace TalentoInterno.CORE.Core.Services;
public class AuthService : IAuthService
{
    private readonly TalentoInternooContext _context;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;
    private readonly IConfiguration _configuration;
    public AuthService(IJwtService jwtService, ILogger<AuthService> logger, IConfiguration configuration, TalentoInternooContext context)
    {
        _jwtService = jwtService;
        _logger = logger;
        _configuration = configuration;
        _context = context;
    }
    public LoginResponseDTO Login(LoginRequestDTO request)
    {
        _logger.LogInformation("Intento de login para usuario {Email}", request.Email);

        // Simulaci�n: en implementaci�n real se valida hash/salt
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



    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        // 1. Buscar usuario por Email
        var usuario = await _context.Usuario
            .Include(u => u.Rol) // Incluir el Rol para el token
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (usuario == null) throw new Exception("Usuario no encontrado.");

        // 2. Verificar Contraseña (Hash)
        bool passwordValida = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash);
        if (!passwordValida) throw new Exception("Contraseña incorrecta.");

        if (usuario.Activo == false) throw new Exception("Usuario inactivo.");

        // 3. Generar Token JWT
        var token = GenerarJwtToken(usuario);

        return new AuthResponseDto
        {
            Token = token,
            Email = usuario.Email,
            Rol = usuario.Rol.Nombre,
            UsuarioId = usuario.UsuarioId,
            ColaboradorId = usuario.ColaboradorId,

        };
    }

    private string GenerarJwtToken(Entities.Usuario usuario)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("id", usuario.UsuarioId.ToString()),
            new Claim(ClaimTypes.Role, usuario.Rol.Nombre),
            new Claim("colaboradorId", usuario.ColaboradorId?.ToString() ?? "")
        }
        ;

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:DurationInMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<AuthResponseDto> GetSessionAsync(int usuarioId)
    {
        // Buscamos al usuario para asegurar que sigue existiendo y activo
        var usuario = await _context.Usuario
            .Include(u => u.Rol)
            .Include(u => u.Colaborador)
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

        if (usuario == null || usuario.Activo == false)
            throw new Exception("Sesión no válida. Usuario no encontrado o inactivo.");

        // Reutilizamos el DTO de respuesta, pero SIN generar un token nuevo
        // (o podrías generar uno nuevo si quisieras renovar el tiempo de vida implícitamente)
        string nombre = usuario.Colaborador != null
            ? $"{usuario.Colaborador.Nombres} {usuario.Colaborador.Apellidos}"
            : "Administrador";

        return new AuthResponseDto
        {
            Token = "", // No devolvemos token nuevo en simple verificación de sesión
            Email = usuario.Email,
            Rol = usuario.Rol.Nombre,
            UsuarioId = usuario.UsuarioId,
            ColaboradorId = usuario.ColaboradorId,
            NombreCompleto = nombre
        };
    }
}

