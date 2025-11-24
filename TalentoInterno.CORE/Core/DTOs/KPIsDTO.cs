using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.DTOs;

public class KPIsDTO
{
    public int VacantesTotales { get; set; }
    public int VacantesCubiertasInternas { get; set; }
    public double TiempoPromedioCobertura { get; set; }
    public List<string> SkillsCriticos { get; set; } = new();
    public Dictionary<string, double> Tendencias { get; set; } = new();
}