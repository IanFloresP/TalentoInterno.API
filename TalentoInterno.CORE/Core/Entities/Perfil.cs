using System;
using System.Collections.Generic;
using TalentoInterno.CORE.Infrastructure.Data;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Perfil
{
    public int PerfilId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Vacante> Vacante { get; set; } = new List<Vacante>();
}
