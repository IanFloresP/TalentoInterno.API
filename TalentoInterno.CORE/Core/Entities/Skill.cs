using System;
using System.Collections.Generic;
using TalentoInterno.CORE.Infrastructure.Data;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Skill
{
    public int SkillId { get; set; }

    public string Nombre { get; set; } = null!;

    public byte TipoSkillId { get; set; }

    public bool? Critico { get; set; }

    public virtual ICollection<ColaboradorSkill> ColaboradorSkill { get; set; } = new List<ColaboradorSkill>();

    public virtual TipoSkill TipoSkill { get; set; } = null!;

    public virtual ICollection<VacanteSkillReq> VacanteSkillReq { get; set; } = new List<VacanteSkillReq>();
}
