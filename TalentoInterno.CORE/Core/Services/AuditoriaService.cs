using Microsoft.Extensions.Logging;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly ILogger<AuditoriaService> _logger;

    public AuditoriaService(ILogger<AuditoriaService> logger)
    {
        _logger = logger;
    }

    public void RegistrarAccion(AuditoriaRegistroDTO registro)
    {
        _logger.LogInformation("Usuario {UsuarioId} realizó acción {Tipo} sobre {Entidad} con Id {IdAfectado} - Resultado: {Resultado} - Rol: {Rol} - IP: {Ip} - Fecha: {Fecha}",
            registro.UsuarioId, registro.Tipo, registro.Entidad, registro.IdAfectado, registro.Resultado, registro.Rol, registro.Ip, registro.Timestamp);
    }

    public AuditoriaResumenDTO ObtenerResumen(int usuarioId, DateTime fechaInicio, DateTime fechaFin)
    {
        return new AuditoriaResumenDTO
        {
            Usuario = $"Usuario {usuarioId}",
            TotalAcciones = 0,
            AccionesPorTipo = new System.Collections.Generic.Dictionary<string, int>(),
            FechaInicio = fechaInicio,
            FechaFin = fechaFin
        };
    }
}