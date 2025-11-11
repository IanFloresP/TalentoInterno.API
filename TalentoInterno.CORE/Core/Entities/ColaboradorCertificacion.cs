using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class ColaboradorCertificacion
{
    public int ColaboradorId { get; set; }

    public int CertificacionId { get; set; }

    public DateOnly? FechaObtencion { get; set; }

    public virtual Certificacion Certificacion { get; set; } = null!;

    public virtual Colaborador Colaborador { get; set; } = null!;
}
