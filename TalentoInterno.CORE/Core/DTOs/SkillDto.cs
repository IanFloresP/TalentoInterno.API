namespace TalentoInterno.CORE.Core.DTOs;

public class SkillDto
{
    public int SkillId { get; set; }
    public string Nombre { get; set; } = null!;
    public byte TipoSkillId { get; set; }
    public bool? Critico { get; set; }
}