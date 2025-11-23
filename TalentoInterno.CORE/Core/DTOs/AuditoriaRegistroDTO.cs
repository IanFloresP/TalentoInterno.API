namespace TalentoInterno.CORE.Core.DTOs;

public class AuditoriaRegistroDTO
{
    public int UsuarioId { get; set; }
    public string Entidad { get; set; } = null!;
    public int IdAfectado { get; set; }
    public string Tipo { get; set; } = null!;
    public string Resultado { get; set; } = null!;
    public string Ip { get; set; } = null!;
    public string Rol { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}