using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Colaborador
{
    public int ColaboradorId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RolId { get; set; }

    public int AreaId { get; set; }

    public int DepartamentoId { get; set; }

    public bool? DisponibleMovilidad { get; set; }

    public bool? Activo { get; set; }

    public DateOnly? FechaAlta { get; set; }

    public virtual Area Area { get; set; } = null!;

    public virtual ICollection<ColaboradorCertificacion> ColaboradorCertificacion { get; set; } = new List<ColaboradorCertificacion>();

    public virtual ICollection<ColaboradorSkill> ColaboradorSkill { get; set; } = new List<ColaboradorSkill>();

    public virtual Departamento Departamento { get; set; } = null!;

    public virtual Rol Rol { get; set; } = null!;
}
