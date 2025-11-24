namespace TalentoInterno.CORE.Core.DTOs;

public class AlertasResponseDto
{
    // Corchete 1: Alertas de Vacantes
    public List<AlertaDto> Vacantes { get; set; } = new();

    // Corchete 2: Alertas de Skills
    public List<AlertaDto> Skills { get; set; } = new();
}