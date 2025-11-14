using System.ComponentModel.DataAnnotations;
namespace TalentoInterno.CORE.Core.DTOs;

public class VacanteUpdateDTO
{
    [Required]
    public string Titulo { get; set; } = null!;
    [Required]
    public int PerfilId { get; set; }
    public int? CuentaId { get; set; }
    public int? ProyectoId { get; set; }
    [Required]
    public DateOnly FechaInicio { get; set; }
    [Required]
    public int UrgenciaId { get; set; }
    [Required]
    public string Estado { get; set; } = null!;
    public string? Descripcion { get; set; }
}