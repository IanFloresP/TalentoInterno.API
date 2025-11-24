using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Auditoria
{
    public int AuditoriaId { get; set; }

    public int? UsuarioId { get; set; }

    public string Accion { get; set; } = null!;

    public string? Entidad { get; set; }

    public int? EntidadId { get; set; }

    public string? Detalle { get; set; }

    public DateTime? Fecha { get; set; }

    public bool? Exitoso { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
