using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class TipoSkill
{
    public byte TipoSkillId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Skill> Skill { get; set; } = new List<Skill>();
}
