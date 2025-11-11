using System;
using System.Collections.Generic;
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Infrastructure.Data;

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
