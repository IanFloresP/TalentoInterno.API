namespace TalentoInterno.CORE.Core.DTOs;

public class VacanteDto
{
    public int VacanteId { get; set; }
    public string Titulo { get; set; } = null!;
    public int PerfilId { get; set; }
    public int? CuentaId { get; set; }
    public int? ProyectoId { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public byte UrgenciaId { get; set; }
    public string Estado { get; set; } = null!;
    public string? Descripcion { get; set; }
}