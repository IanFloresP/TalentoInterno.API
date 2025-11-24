using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Rol
{
    public int RolId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Colaborador> Colaborador { get; set; } = new List<Colaborador>();

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
