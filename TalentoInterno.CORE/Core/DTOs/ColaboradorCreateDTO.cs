using System.ComponentModel.DataAnnotations;

namespace TalentoInterno.CORE.Core.DTOs;

public class ColaboradorCreateDTO
{
    // --- DATOS PARA LA TABLA COLABORADOR ---

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombres { get; set; } = null!;

    [Required(ErrorMessage = "El DNI es obligatorio")]
    public string DNI { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    public string Apellidos { get; set; } = null!;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "El Rol es obligatorio")]
    public int RolId { get; set; }

    [Required(ErrorMessage = "El Área es obligatoria")]
    public int AreaId { get; set; }

    [Required(ErrorMessage = "El Departamento es obligatorio")]
    public int DepartamentoId { get; set; }

    public bool? DisponibleMovilidad { get; set; }

    public bool? Activo { get; set; }

    // --- DATOS PARA LA TABLA USUARIO (Seguridad) ---
    // Esta propiedad NO se guarda en la tabla Colaborador, 
    // el servicio la usa para crear el registro en la tabla Usuario.

    [Required(ErrorMessage = "La contraseña es obligatoria para registrar al usuario")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string Password { get; set; } = null!;
}