using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Cuenta
{
    public int CuentaId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Proyecto> Proyecto { get; set; } = new List<Proyecto>();

    public virtual ICollection<Vacante> Vacante { get; set; } = new List<Vacante>();
}
