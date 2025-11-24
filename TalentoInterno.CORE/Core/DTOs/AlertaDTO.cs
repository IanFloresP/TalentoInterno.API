using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.DTOs;

public class AlertaDTO
{
    public string Tipo { get; set; } = null!;
    public string Severidad { get; set; } = null!;
    public string Mensaje { get; set; } = null!;
    public string Detalles { get; set; } = null!;
    public List<string> Sugerencias { get; set; } = new();
}