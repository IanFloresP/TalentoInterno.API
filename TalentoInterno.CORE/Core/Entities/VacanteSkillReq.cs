using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class VacanteSkillReq
{
    public int VacanteId { get; set; }

    public int SkillId { get; set; }

    public byte NivelDeseado { get; set; }

    public decimal? Peso { get; set; }

    public bool? Critico { get; set; }

    public virtual NivelDominio NivelDeseadoNavigation { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;

    public virtual Vacante Vacante { get; set; } = null!;
}
