using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.API.Filters;

public class AuditoriaFilter : IAsyncActionFilter
{
    private readonly IAuditoriaService _auditoriaService;

    public AuditoriaFilter(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 1. Ejecutar la acción (el controlador hace su trabajo)
        var executedContext = await next();

        // 2. Después de que termina, registramos qué pasó
        try
        {
            // Obtener el usuario del Token (si existe)
            var userIdClaim = context.HttpContext.User.FindFirst("id")?.Value;
            int? userId = userIdClaim != null ? int.Parse(userIdClaim) : null;

            // Obtener detalles de la petición
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();
            var method = context.HttpContext.Request.Method;

            // Determinar si fue exitoso (Status 2xx)
            bool exitoso = executedContext.Exception == null;

            // Preparar el mensaje
            string detalle = $"Método: {method} | Endpoint: {controllerName}/{actionName}";
            if (executedContext.Exception != null)
            {
                detalle += $" | Error: {executedContext.Exception.Message}";
            }

            // ¡REGISTRO AUTOMÁTICO!
            await _auditoriaService.RegistrarAccionAsync(new AuditoriaCreateDto
            {
                UsuarioId = userId,
                Accion = actionName?.ToUpper() ?? "DESCONOCIDA",
                Entidad = controllerName,
                Detalle = detalle,
                Exitoso = exitoso
            });
        }
        catch
        {
            // Si falla la auditoría, no queremos romper la respuesta al usuario,
            // así que ignoramos el error de logueo (o lo mandamos a consola).
        }
    }
}