namespace TalentoInterno.CORE.Core.DTOs;

public class ColaboradorDTO
{
    public int ColaboradorId { get; set; }
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int RolId { get; set; }
    public int AreaId { get; set; }
    public int DepartamentoId { get; set; }
    public bool? DisponibleMovilidad { get; set; }
    public bool? Activo { get; set; }
    public DateOnly? FechaAlta { get; set; }

    // Propiedades relacionadas
    public string? AreaNombre { get; set; }
    public string? DepartamentoNombre { get; set; }
    public string? RolNombre { get; set; }
    public IEnumerable<string>? Certificaciones { get; set; }
    public IEnumerable<string>? Skills { get; set; }
}