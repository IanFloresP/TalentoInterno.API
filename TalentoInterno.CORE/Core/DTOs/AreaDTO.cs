using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentoInterno.CORE.Core.DTOs
{
    public class AreaDTO
    {
        public int AreaId { get; set; }
        public string Nombre { get; set; } = null!;
        public int? DepartamentoId { get; set; } // Opcional, si quieres mostrar a qué depto. pertenece
    }
}


