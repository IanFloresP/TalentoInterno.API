using System.ComponentModel.DataAnnotations;

namespace TalentoInterno.CORE.Core.DTOs;

public class SkillCreateDTO
{
    [Required]
    public string Nombre { get; set; } = null!;

    [Required]
    public byte TipoSkillId { get; set; }

    public bool? Critico { get; set; }
}