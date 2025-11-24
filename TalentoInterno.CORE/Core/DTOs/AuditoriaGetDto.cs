namespace TalentoInterno.CORE.Core.DTOs;

public class AuditoriaGetDto
{
    public int AuditoriaId { get; set; }
    public string? UsuarioEmail { get; set; } // Queremos ver el email, no solo el ID
    public string Accion { get; set; } = null!;
    public string? Entidad { get; set; }
    public int? EntidadId { get; set; }
    public string? Detalle { get; set; }
    public DateTime Fecha { get; set; }
    public bool? Exitoso { get; set; }
}