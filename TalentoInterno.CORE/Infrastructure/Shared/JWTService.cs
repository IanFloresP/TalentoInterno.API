using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Settings;

namespace TalentoInterno.CORE.Core.Services; // Asegúrate del namespace correcto

public class JwtService : IJwtService
{
    public JWTSettings _settings { get; }

    // Usamos IOptionsSnapshot o IOptions para inyección de dependencias limpia
    public JwtService(IOptions<JWTSettings> settings)
    {
        _settings = settings.Value;
    }

    public string GenerateJWToken(Colaborador colaborador)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, colaborador.Email ?? ""),
            new Claim("id", colaborador.ColaboradorId.ToString()),
            new Claim(ClaimTypes.Role, colaborador.Rol?.Nombre ?? "SinRol")
        };

        // 4. Crear el Token
        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.DurationInMinutes),
            signingCredentials: creds
        );

        // 5. Escribirlo como string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}