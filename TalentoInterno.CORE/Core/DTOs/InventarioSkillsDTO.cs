using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.DTOs;

public class InventarioSkillsDTO
{
    public string Area { get; set; } = null!;
    public string Rol { get; set; } = null!;
    public List<string> Skills { get; set; } = new();
    public Dictionary<string, int> TotalesPorSkill { get; set; } = new();
    public List<string> Brechas { get; set; } = new();
}