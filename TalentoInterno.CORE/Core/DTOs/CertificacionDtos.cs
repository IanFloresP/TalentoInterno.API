using System.ComponentModel.DataAnnotations;

namespace TalentoInterno.CORE.Core.DTOs;

// Para Lectura (GET)
public class CertificacionDto
{
    public int CertificacionId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
}

// Para Creación (POST)
public class CertificacionCreateDto
{
    [Required]
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
}

// Para Edición (PUT)
public class CertificacionUpdateDto
{
    [Required]
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
}