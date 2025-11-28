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

    // --- NUEVOS CAMPOS ---
    public int? AreaId { get; set; }         // Opcional o [Required] según tu regla
    public int? DepartamentoId { get; set; } // Opcional o [Required] según tu regla
    // ---------------------
    public string? Descripcion { get; set; }
}