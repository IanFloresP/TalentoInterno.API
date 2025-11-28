namespace TalentoInterno.CORE.Core.DTOs;

public class VacanteListDTO
{
    public int VacanteId { get; set; }
    public string Titulo { get; set; } = null!;
    public string AreaNombre { get; set; } = null!;
    public string? DepartamentoNombre { get; set; } // <--- ¡AQUÍ!
    public string? PerfilNombre { get; set; }
    public string? UrgenciaNombre { get; set; }
    public string Estado { get; set; } = null!;
    public DateOnly FechaInicio { get; set; }
}