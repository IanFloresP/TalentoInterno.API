using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Area
{
    public int AreaId { get; set; }

    public string Nombre { get; set; } = null!;

    public int? DepartamentoId { get; set; } // Nueva columna

    public virtual Departamento? Departamento { get; set; } // Relación con Departamento

    public virtual ICollection<Colaborador> Colaborador { get; set; } = new List<Colaborador>();
}
