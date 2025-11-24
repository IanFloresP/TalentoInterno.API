using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace TalentoInterno.CORE.Core.Services;

public class AuthService : IAuthService
{
    private readonly TalentoInternooContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(TalentoInternooContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
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
            ColaboradorId = usuario.ColaboradorId
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