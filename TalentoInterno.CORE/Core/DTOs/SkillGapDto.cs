namespace TalentoInterno.CORE.Core.DTOs;

public class SkillGapDto
{
    public int SkillId { get; set; }
    public string SkillNombre { get; set; } = null!;
    public byte NivelDeseado { get; set; }
    public int AvailableCount { get; set; }
    public int Gap { get; set; }
    public bool Critico { get; set; }
    public bool RecruitmentAlert { get; set; }
}
