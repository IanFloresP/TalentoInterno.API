namespace TalentoInterno.CORE.Core.DTOs;

public class AuditoriaCreateDto
{
    public int? UsuarioId { get; set; }
    public string Accion { get; set; } = null!;
    public string? Entidad { get; set; }
    public int? EntidadId { get; set; }
    public string? Detalle { get; set; }
    public bool Exitoso { get; set; } = true;
}