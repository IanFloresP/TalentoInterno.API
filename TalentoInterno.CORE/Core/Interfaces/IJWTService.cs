using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Settings;

public interface IJwtService
{
    JWTSettings _settings { get; }

    string GenerateJWToken(Colaborador colaborador);
}