namespace TalentoInterno.CORE.Core.DTOs;

public class VacanteSkillReqDto
{
    public int VacanteId { get; set; }
    public int SkillId { get; set; }
    public string SkillName { get; set; }
    public int NivelDeseado { get; set; }
    public decimal? Peso { get; set; }
    public bool? Critico { get; set; }
}