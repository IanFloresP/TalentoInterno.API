using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IAuditoriaService
{
    void RegistrarAccion(AuditoriaRegistroDTO registro);
    AuditoriaResumenDTO ObtenerResumen(int usuarioId, DateTime fechaInicio, DateTime fechaFin);
}