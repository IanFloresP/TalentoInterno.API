using System.ComponentModel.DataAnnotations;

namespace TalentoInterno.CORE.Core.DTOs;

public class ColaboradorCertificacionCreateDto
{
    [Required]
    public int CertificacionId { get; set; }

    [Required]
    public DateOnly FechaObtencion { get; set; }
}