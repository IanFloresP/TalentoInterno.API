using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Certificacion
{
    public int CertificacionId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<ColaboradorCertificacion> ColaboradorCertificacion { get; set; } = new List<ColaboradorCertificacion>();
}
