using System.ComponentModel.DataAnnotations;
namespace TalentoInterno.CORE.Core.DTOs;

public class VacanteSkillReqGetDTO
{
    public int VacanteId { get; set; }
    public int SkillId { get; set; }
    public string SkillNombre { get; set; } = null!;
    public int NivelDeseado { get; set; }
    public string NivelNombre { get; set; } = null!;
    public decimal? Peso { get; set; }
    public bool? Critico { get; set; }
}
