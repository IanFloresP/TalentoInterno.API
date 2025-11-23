namespace TalentoInterno.CORE.Core.DTOs;

public class ColaboradorSkillDto
{
    public int ColaboradorId { get; set; }
    public int SkillId { get; set; }
    public string? SkillNombre { get; set; }
    public byte NivelId { get; set; }
    public string? NivelNombre { get; set; }
    public decimal? AniosExp { get; set; }
}
