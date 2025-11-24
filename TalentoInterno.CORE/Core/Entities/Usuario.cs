using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RolId { get; set; }

    public int? ColaboradorId { get; set; }

    public bool? Activo { get; set; }

    public DateOnly? FechaCreacion { get; set; }

    public virtual ICollection<Auditoria> Auditoria { get; set; } = new List<Auditoria>();

    public virtual Colaborador? Colaborador { get; set; }

    public virtual Rol Rol { get; set; } = null!;
}
