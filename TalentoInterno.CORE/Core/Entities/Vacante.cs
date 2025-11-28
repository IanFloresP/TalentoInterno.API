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

    public int UrgenciaId { get; set; }

    public string Estado { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int? AreaId { get; set; }

    public int? DepartamentoId { get; set; }

    public virtual Area? Area { get; set; }

    public virtual Cuenta? Cuenta { get; set; }

    public virtual Departamento? Departamento { get; set; }

    public virtual Perfil Perfil { get; set; } = null!;

    public virtual ICollection<Postulacion> Postulacion { get; set; } = new List<Postulacion>();

    public virtual Proyecto? Proyecto { get; set; }

    public virtual Urgencia Urgencia { get; set; } = null!;

    public virtual ICollection<VacanteSkillReq> VacanteSkillReq { get; set; } = new List<VacanteSkillReq>();
}
