namespace TalentoInterno.CORE.Core.DTOs;

public class MatchResultDTO
{
    public int ColaboradorId { get; set; }
    public int VacanteId { get; set; }
    public string Nombre { get; set; } = null!;

    // --- LO NUEVO QUE TE FALTA (Para el Excel) ---
    public string? Email { get; set; }            // Para contactarlo
    public string? AreaNombre { get; set; }       // Para filtrar por Área (ej: Sistemas)
    public string? DepartamentoNombre { get; set; } // Para filtrar por Depto (ej: Desarrollo)
    public string? RolNombre { get; set; }        // Para saber qué puesto tiene hoy
    // ---------------------------------------------

    public double PorcentajeMatch { get; set; }
    public List<SkillMatchDetalleDTO> SkillsQueCumple { get; set; } = new();
    public List<SkillMatchDetalleDTO> SkillsFaltantes { get; set; } = new();
}

// La clase hija se queda igual, está bien.
public class SkillMatchDetalleDTO
{
    public int SkillId { get; set; }
    public string Nombre { get; set; } = null!;
    public int NivelRequeridoId { get; set; }
    public string? NivelRequeridoNombre { get; set; }
    public int? NivelColaboradorId { get; set; }
    public string? NivelColaboradorNombre { get; set; }
    public bool CumpleNivel { get; set; }
}