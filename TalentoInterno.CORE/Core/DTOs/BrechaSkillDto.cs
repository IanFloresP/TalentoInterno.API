namespace TalentoInterno.CORE.Core.DTOs;

public class BrechaSkillDto
{
    public int ColaboradorId { get; set; }
    public string NombreColaborador { get; set; } = null!;
    public string SkillNombre { get; set; } = null!;
    public string NivelRequerido { get; set; } = null!;
    public int NivelRequeridoId { get; set; }
    public string NivelActual { get; set; } = null!;
    public int NivelActualId { get; set; }
    public double BrechaPorcentaje { get; set; }
}