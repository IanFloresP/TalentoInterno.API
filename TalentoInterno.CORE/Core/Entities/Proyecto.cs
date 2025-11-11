using System;
using System.Collections.Generic;
using TalentoInterno.CORE.Infrastructure.Data;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Proyecto
{
    public int ProyectoId { get; set; }

    public string Nombre { get; set; } = null!;

    public int? CuentaId { get; set; }

    public virtual Cuenta? Cuenta { get; set; }

    public virtual ICollection<Vacante> Vacante { get; set; } = new List<Vacante>();
}
