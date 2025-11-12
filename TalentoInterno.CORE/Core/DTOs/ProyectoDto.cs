namespace TalentoInterno.CORE.Core.DTOs;

public class ProyectoDto
{
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = null!;
    public int? CuentaId { get; set; }
}