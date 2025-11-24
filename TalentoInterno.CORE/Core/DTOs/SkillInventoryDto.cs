namespace TalentoInterno.CORE.Core.DTOs;

public class SkillInventoryDto
{
    public string SkillNombre { get; set; } = null!;
    public string NivelNombre { get; set; } = null!;
    public string AreaNombre { get; set; } = null!;
    public int CantidadPersonas { get; set; }
}