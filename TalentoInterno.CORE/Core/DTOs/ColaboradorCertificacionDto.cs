namespace TalentoInterno.CORE.Core.DTOs;

public class ColaboradorCertificacionDto
{
    public int ColaboradorId { get; set; }
    public int CertificacionId { get; set; }
    public string NombreCertificacion { get; set; } = null!;
    public DateOnly? FechaObtencion { get; set; }
    // Si tienes fecha de expiración en BD, añádela aquí
}