using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Settings;
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Infrastructure.Shared
{
    public class JWTService : IJWTService
    {
        public JWTSettings _settings { get; }
        public JWTService(IOptions<JWTSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateJWToken(Colaborador colaborador)
        {
            var ssk = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var sc = new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(sc);

            var claims = new[] {
              new Claim(ClaimTypes.Name, (colaborador.Nombres ?? "") + " " + (colaborador.Apellidos ?? "")),
              new Claim(ClaimTypes.GivenName, colaborador.Nombres ?? ""),
              new Claim(ClaimTypes.Email, colaborador.Email ?? ""),
              new Claim(ClaimTypes.Role, (colaborador.Rol != null && colaborador.Rol.Nombre == "A") ? "Admin" : "User"),
              new Claim("ColaboradorId", colaborador.ColaboradorId.ToString()),
          };

            var payload = new JwtPayload(
                            _settings.Issuer
                            , _settings.Audience
                            , claims
                            , DateTime.UtcNow
                            , DateTime.UtcNow.AddMinutes(_settings.DurationInMinutes));

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

