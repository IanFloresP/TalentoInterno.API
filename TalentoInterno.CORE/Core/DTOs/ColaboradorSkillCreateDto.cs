using System.ComponentModel.DataAnnotations;

namespace TalentoInterno.CORE.Core.DTOs;

public class ColaboradorSkillCreateDTO
{

    [Required]
    public int SkillId { get; set; }

    [Required]
    public byte NivelId { get; set; } // Es 'byte' según tu entidad

    [Range(0, 50)]
    public decimal? AniosExp { get; set; } // Es 'decimal?' según tu entidad
}