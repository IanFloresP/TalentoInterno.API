using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Vacante
{
    public int VacanteId { get; set; }

    public string Titulo { get; set; } = null!;

    public int PerfilId { get; set; }

    public int? CuentaId { get; set; }

    public int? ProyectoId { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public byte UrgenciaId { get; set; }

    public string Estado { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual Cuenta? Cuenta { get; set; }

    public virtual Perfil Perfil { get; set; } = null!;

    public virtual Proyecto? Proyecto { get; set; }

    public virtual Urgencia Urgencia { get; set; } = null!;

    public virtual ICollection<VacanteSkillReq> VacanteSkillReq { get; set; } = new List<VacanteSkillReq>();
}
