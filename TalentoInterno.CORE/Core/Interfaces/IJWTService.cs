using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Settings;

namespace TalentoInterno.CORE.Core.Interfaces
{
    public interface IJWTService
    {
        JWTSettings _settings { get; }

        string GenerateJWToken(Colaborador colaborador);
    }
}