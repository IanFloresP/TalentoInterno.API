namespace TalentoInterno.CORE.Core.DTOs;

public class AuditoriaResumenDto
{
    public string Usuario { get; set; } = null!; // Email o Nombre
    public int CantidadAcciones { get; set; }
    public string UltimaAccion { get; set; } = null!;
    public DateTime FechaUltimaAccion { get; set; }
}