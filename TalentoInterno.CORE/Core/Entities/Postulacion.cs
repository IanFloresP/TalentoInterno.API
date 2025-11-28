using System;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Entities;

public partial class Postulacion
{
    public int PostulacionId { get; set; }

    public int VacanteId { get; set; }

    public int ColaboradorId { get; set; }

    public DateTime? FechaPostulacion { get; set; }

    public string? Estado { get; set; }

    public string? Comentarios { get; set; }

    public decimal? MatchScore { get; set; }

    public virtual Colaborador Colaborador { get; set; } = null!;

    public virtual Vacante Vacante { get; set; } = null!;
}
