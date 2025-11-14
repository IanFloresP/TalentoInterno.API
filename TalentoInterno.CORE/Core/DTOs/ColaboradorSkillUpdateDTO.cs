using System.ComponentModel.DataAnnotations;

namespace TalentoInterno.CORE.Core.DTOs;

public class ColaboradorSkillUpdateDTO
{
    [Required]
    public byte NivelId { get; set; }

    [Range(0, 50)]
    public decimal? AniosExp { get; set; }
}