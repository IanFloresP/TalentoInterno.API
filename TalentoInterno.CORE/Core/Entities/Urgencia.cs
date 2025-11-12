using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Urgencia
{
    public byte UrgenciaId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Vacante> Vacante { get; set; } = new List<Vacante>();
}
