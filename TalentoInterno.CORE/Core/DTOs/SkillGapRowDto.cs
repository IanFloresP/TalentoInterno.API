namespace TalentoInterno.CORE.Core.DTOs;

public class SkillGapRowDto
{
    public int ColaboradorId { get; set; }
    public string ColaboradorNombre { get; set; } = null!;
    public int SkillId { get; set; }
    public string SkillNombre { get; set; } = null!;
    public byte NivelDeseado { get; set; }
    public byte NivelColaboradorId { get; set; }
    public string? NivelColaboradorNombre { get; set; }
    public int Brecha { get; set; }
    public double BrechaPct { get; set; }
    public int AvailableCount { get; set; }
    public bool Critico { get; set; }
    public bool RecruitmentAlert { get; set; }
}