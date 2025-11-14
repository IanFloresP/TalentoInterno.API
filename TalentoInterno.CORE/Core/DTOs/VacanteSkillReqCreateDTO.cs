namespace TalentoInterno.CORE.Core.DTOs;

public class VacanteSkillReqCreateDTO
{
    public int SkillId { get; set; }
    public byte NivelDeseado { get; set; }
    public decimal? Peso { get; set; }
    public bool? Critico { get; set; }
}