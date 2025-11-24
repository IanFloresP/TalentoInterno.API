using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Settings;

public class JwtService : IJwtService
{
    public JWTSettings _settings { get; }
    private readonly SymmetricSecurityKey _key;

    public JwtService(IConfiguration config)
    {
        _settings = new JWTSettings
        {
            SecretKey = config["JwtSettings:SecretKey"] ?? throw new ArgumentNullException("JwtSettings:SecretKey"),
            Issuer = config["JwtSettings:Issuer"],
            Audience = config["JwtSettings:Audience"],
            DurationInMinutes = double.Parse(config["JwtSettings:DurationInMinutes"] ?? "60")
        };
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
    }

    public string GenerateJWToken(Colaborador colaborador)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, colaborador.Email ?? ""),
            new Claim("id", colaborador.ColaboradorId.ToString()),
            new Claim(ClaimTypes.Role, colaborador.Rol?.Nombre ?? "")
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.DurationInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

