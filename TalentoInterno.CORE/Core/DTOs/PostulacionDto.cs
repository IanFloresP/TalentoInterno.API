using System.ComponentModel.DataAnnotations;

namespace TalentoInterno.CORE.Core.DTOs;

// DTO de Lectura
public class PostulacionDto
{
    public int PostulacionId { get; set; }
    public int VacanteId { get; set; }
    public string VacanteTitulo { get; set; } = null!;
    public int ColaboradorId { get; set; }
    public string NombreColaborador { get; set; } = null!;
    public string Estado { get; set; } = null!; // "En Revisión", "Entrevista", "Seleccionado", "Rechazado"
    public decimal? MatchScore { get; set; } // El score histórico
    public DateTime FechaPostulacion { get; set; }
    public string? Comentarios { get; set; }
}

// DTO para Postulación Masiva (Profesional)
public class CrearPostulacionMasivaDto
{
    [Required]
    public int VacanteId { get; set; }

    [Required]
    public List<int> ColaboradorIds { get; set; } = new(); // Lista de IDs seleccionados en el Frontend
}

// DTO para Cambio de Estado (Aprobar/Entrevistar)
public class CambiarEstadoDto
{
    [Required]
    public string NuevoEstado { get; set; } = null!; // Ej: "Entrevista", "Seleccionado"
    public string? Comentarios { get; set; }
}

// DTO para Rechazo
public class RechazarCandidatoDto
{
    [Required]
    public string MotivoRechazo { get; set; } = null!;
}