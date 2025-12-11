using Microsoft.AspNetCore.Http; // Necesario para IFormFile
using System.ComponentModel.DataAnnotations;

namespace TalentoInterno.CORE.Core.DTOs;

public class EmailComposeDto
{
    [Required]
    [EmailAddress]
    public string Destinatario { get; set; }

    [Required]
    public string Asunto { get; set; }

    public string Cuerpo { get; set; } // Puede ser HTML

    // Aquí está la clave: Una lista de archivos
    public List<IFormFile>? Adjuntos { get; set; }
}