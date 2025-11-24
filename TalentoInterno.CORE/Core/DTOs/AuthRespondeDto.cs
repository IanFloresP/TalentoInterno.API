using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentoInterno.CORE.Core.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!; // Ej: "Admin", "Reclutador"
        public int UsuarioId { get; set; }
        public int? ColaboradorId { get; set; } // Para saber quién es
        public string? NombreCompleto { get; set; } // Nombre del colaborador asociado
    }
}
