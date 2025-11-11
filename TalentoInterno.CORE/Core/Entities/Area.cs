using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Area
{
    public int AreaId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Colaborador> Colaborador { get; set; } = new List<Colaborador>();
}
