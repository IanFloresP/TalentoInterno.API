using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class ColaboradorSkill
{
    public int ColaboradorId { get; set; }

    public int SkillId { get; set; }

    public byte NivelId { get; set; }

    public decimal? AniosExp { get; set; }

    public virtual Colaborador Colaborador { get; set; } = null!;

    public virtual NivelDominio Nivel { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
