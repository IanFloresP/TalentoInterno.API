namespace TalentoInterno.CORE.Core.DTOs;

public class VacanteGetDTO
{
    public int VacanteId { get; set; }
    public string Titulo { get; set; } = null!;
    public string? PerfilNombre { get; set; } // De Perfil.Nombre
    public string? CuentaNombre { get; set; } // De Cuenta.Nombre
    public string? ProyectoNombre { get; set; } // De Proyecto.Nombre
    public DateOnly FechaInicio { get; set; }
    public string? UrgenciaNombre { get; set; } // De Urgencia.Nombre
    public string Estado { get; set; } = null!;
    public string? Descripcion { get; set; }

}