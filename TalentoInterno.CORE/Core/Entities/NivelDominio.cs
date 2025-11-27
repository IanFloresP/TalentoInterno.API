using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class NivelDominio
{
    public int NivelId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<ColaboradorSkill> ColaboradorSkill { get; set; } = new List<ColaboradorSkill>();

    public virtual ICollection<VacanteSkillReq> VacanteSkillReq { get; set; } = new List<VacanteSkillReq>();
}
