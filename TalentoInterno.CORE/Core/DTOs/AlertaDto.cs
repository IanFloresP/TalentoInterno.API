namespace TalentoInterno.CORE.Core.DTOs;

public class AlertaDto
{
    public string Tipo { get; set; } = null!; // "RIESGO_VACANTE" o "ESCASEZ_SKILL"
    public string Mensaje { get; set; } = null!;
    public string Criticidad { get; set; } = null!;  // Alta, Media, Baja
    public int? EntidadId { get; set; } // ID de la Vacante o Skill
    public string EntidadNombre { get; set; } = null!; // Nombre de la Vacante o Skill
    public int ValorActual { get; set; } // Ej: 0 candidatos
    public string? Detalles { get; set; } // Información adicional
    public List<string>? Sugerencias { get; set; } // Recomendaciones

}