using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentoInterno.CORE.Core.DTOs
{

    public class PostulacionDto
    {
        public int PostulacionId { get; set; }
        public int VacanteId { get; set; }
        public string VacanteTitulo { get; set; } = null!;
        public int ColaboradorId { get; set; }
        public string NombreColaborador { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public decimal? MatchScore { get; set; }
        public DateTime FechaPostulacion { get; set; }
        public string? Comentarios { get; internal set; }
    }

    public class CrearPostulacionDto
    {
        public int VacanteId { get; set; }
        public int ColaboradorId { get; set; }
        public decimal MatchScore { get; set; } // Lo guardamos para referencia
    }

    public class CambiarEstadoDto
    {
        public string NuevoEstado { get; set; } = null!; // Ej: "Seleccionado"
        public string? Comentarios { get; set; }
    }
}