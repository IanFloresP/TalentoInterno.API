namespace TalentoInterno.CORE.Core.DTOs;

public class AuditoriaResumenDTO
{
    public string Usuario { get; set; } = null!;
    public int TotalAcciones { get; set; }
    public System.Collections.Generic.Dictionary<string, int> AccionesPorTipo { get; set; } = new();
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}