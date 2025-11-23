namespace TalentoInterno.CORE.Core.DTOs;

public class ReporteRequestDTO
{
    public string Formato { get; set; } = null!;
    public string? Area { get; set; }
    public string? Rol { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? VacanteId { get; set; }
}