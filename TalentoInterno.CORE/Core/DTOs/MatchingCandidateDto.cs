namespace TalentoInterno.CORE.Core.DTOs;

public class MatchingCandidateDto
{
    public int ColaboradorId { get; set; }
    public string Nombre { get; set; } = null!;
    public decimal MatchScore { get; set; }
    public IEnumerable<ColaboradorSkillCreateDTO>? Skills { get; set; }
    public bool RecruitmentAlert { get; set; }
}
