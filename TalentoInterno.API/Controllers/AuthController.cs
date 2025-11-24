using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// Asegúrate de tener este using para la manipulación de texto
using System.Text.RegularExpressions;
using TalentoInterno.API.Filters;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly TalentoInternooContext _context; // <-- Agrega esto
    private readonly IAuditoriaService _auditoriaService;

    public AuthController(IAuthService authService, TalentoInternooContext context, IAuditoriaService auditoriaService) // <-- Inyéctalo
    {
        _authService = authService;
        _context = context;
        _auditoriaService = auditoriaService;
    }

    [HttpPost("login")] // HU-16
    //[ServiceFilter(typeof(AuditoriaFilter))]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var respuesta = await _authService.LoginAsync(loginDto);

            // 3. ¡REGISTRO MANUAL AQUÍ! (Porque aquí sí sabemos quién es)
            await _auditoriaService.RegistrarAccionAsync(new TalentoInterno.CORE.Core.DTOs.AuditoriaCreateDto
            {
                UsuarioId = respuesta.UsuarioId, // <-- Usamos el ID real
                Accion = "LOGIN",
                Entidad = "Auth",
                Detalle = "Inicio de sesión exitoso",
                Exitoso = true
            });

            return Ok(respuesta);
        }
        catch (Exception ex)
        {
            // Opcional: Registrar intento fallido como Anónimo
            return Unauthorized(new { message = ex.Message });
        }
    }
    [HttpGet("session")]
    [Authorize] // <--- ¡Esto protege el endpoint! Solo entra si hay Token válido.
    public async Task<IActionResult> GetSession()
    {
        try
        {
            // Extraemos el ID del usuario desde el Token que vino en la cabecera
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            int usuarioId = int.Parse(userIdClaim);

            // Consultamos a la BD para ver si el usuario sigue activo
            var sessionData = await _authService.GetSessionAsync(usuarioId);

            return Ok(sessionData);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = "Sesión expirada o inválida.", details = ex.Message });
        }
    }

    // HU-16: Logout
    [HttpPost("logout")]
    [Authorize] // Opcional: Solo usuarios logueados pueden hacer logout
    public IActionResult Logout()
    {
        // En JWT estándar, el servidor no "borra" el token (porque lo tiene el cliente).
        // Pero este endpoint sirve para que el Frontend limpie sus cookies/localStorage.
        // También podríamos añadir el token a una "Lista Negra" en BD si quisieras máxima seguridad.

        return Ok(new { message = "Sesión cerrada exitosamente." });
    }


    //Para migrar colaboradores a usuarios
    [HttpPost("migracion")]
    public async Task<IActionResult> MigrarUsuariosGeneral()
    {
        // 1. Buscar todos los colaboradores que NO tienen un registro en la tabla Usuario
        //    (Hacemos un "Left Join" lógico y filtramos los nulos)
        var colaboradoresSinUsuario = await _context.Colaborador
            .Where(c => !_context.Usuario.Any(u => u.ColaboradorId == c.ColaboradorId))
            .ToListAsync();

        if (!colaboradoresSinUsuario.Any())
        {
            return Ok("No se encontraron colaboradores pendientes de migración.");
        }

        int creados = 0;
        var logs = new List<string>();

        foreach (var colab in colaboradoresSinUsuario)
        {
            // 2. Generar la contraseña automáticamente (Patrón: nombreapellido123)
            //    Limpiamos espacios para evitar errores (ej: "Juan Jose" -> "juanjose")
            string nombreLimpio = Regex.Replace(colab.Nombres, @"\s+", "").ToLower();
            string apellidoLimpio = Regex.Replace(colab.Apellidos, @"\s+", "").ToLower();
            string passwordGenerada = $"{nombreLimpio}{apellidoLimpio}123";

            var nuevoUsuario = new TalentoInterno.CORE.Core.Entities.Usuario
            {
                Email = colab.Email,
                // 3. Hashear la contraseña generada
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordGenerada),
                RolId = colab.RolId, // Heredamos el mismo rol que tiene en RRHH
                ColaboradorId = colab.ColaboradorId,
                Activo = colab.Activo ?? true,
                FechaCreacion = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Usuario.Add(nuevoUsuario);
            creados++;
            logs.Add($"Creado usuario para: {colab.Email} | Pass temporal: {passwordGenerada}");
        }

        // 4. Guardar todos los cambios en una sola transacción
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Mensaje = $"Se migraron exitosamente {creados} usuarios.",
            Detalle = logs
        });
    }

}