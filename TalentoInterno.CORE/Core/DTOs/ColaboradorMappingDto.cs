namespace TalentoInterno.CORE.Core.DTOs;

public class ColaboradorMappingDto
{
    public int ColaboradorId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Rol { get; set; }
    public List<ColaboradorSkillDto> Skills { get; set; } = new List<ColaboradorSkillDto>();
}
