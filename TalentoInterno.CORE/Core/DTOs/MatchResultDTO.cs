namespace TalentoInterno.CORE.Core.DTOs;

public class MatchResultDTO
{
    public int ColaboradorId { get; set; }
    public int VacanteId { get; set; }
    public double PorcentajeMatch { get; set; }
    public List<SkillMatchDetalleDTO> SkillsQueCumple { get; set; } = new();
    public List<SkillMatchDetalleDTO> SkillsFaltantes { get; set; } = new();
}

public class SkillMatchDetalleDTO
{
    public int SkillId { get; set; }
    public string Nombre { get; set; } = null!;
    public byte NivelRequeridoId { get; set; }
    public string? NivelRequeridoNombre { get; set; } // De VacanteSkillReq.NivelDeseadoNavigation
    public byte? NivelColaboradorId { get; set; }
    public string? NivelColaboradorNombre { get; set; } // De ColaboradorSkill.Nivel
    public bool CumpleNivel { get; set; }
}