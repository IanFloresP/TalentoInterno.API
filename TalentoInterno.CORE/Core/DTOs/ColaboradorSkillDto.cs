namespace TalentoInterno.CORE.Core.DTOs;

public class ColaboradorSkillDto
{
    public int ColaboradorId { get; set; }
    public int SkillId { get; set; }
    public string? SkillNombre { get; set; }
   
    public int TipoSkillId { get; set; }
    public string? TipoSkillNombre { get; set; }
   
    public int NivelId { get; set; }
    public string? NivelNombre { get; set; }
    public decimal? AniosExp { get; set; }
}
