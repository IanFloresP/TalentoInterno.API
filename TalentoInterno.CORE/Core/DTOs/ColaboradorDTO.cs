using System.ComponentModel.DataAnnotations;

namespace TalentoInterno.CORE.Core.DTOs;

public class ColaboradorDTO
{
    public int ColaboradorId { get; set; }
    public string Dni { get; set; } = null!; // <--- NUEVO
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(8)] // Añadir validación de longitud mínima
    public string? Contraseña { get; set; } = null!;// Campo de texto plano para la nueva contraseña
    public int RolId { get; set; }
    public int AreaId { get; set; }
    public int DepartamentoId { get; set; }
    public bool? DisponibleMovilidad { get; set; }
    public bool? Activo { get; set; }
    public DateOnly? FechaAlta { get; set; }
    public string? AreaNombre { get; set; }
    public string? DepartamentoNombre { get; set; }
    public string? RolNombre { get; set; }
    public IEnumerable<string>? Certificaciones { get; set; }
    public IEnumerable<string>? Skills { get; set; }
}