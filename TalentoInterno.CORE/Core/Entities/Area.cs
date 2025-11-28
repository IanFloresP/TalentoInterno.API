using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Area
{
    public int AreaId { get; set; }

    public string Nombre { get; set; } = null!;

    public int? DepartamentoId { get; set; }

    public virtual ICollection<Colaborador> Colaborador { get; set; } = new List<Colaborador>();

    public virtual Departamento? Departamento { get; set; }

    public virtual ICollection<Vacante> Vacante { get; set; } = new List<Vacante>();
}
