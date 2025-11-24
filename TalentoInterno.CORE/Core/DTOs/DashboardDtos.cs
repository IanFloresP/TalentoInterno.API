namespace TalentoInterno.CORE.Core.DTOs;

public class DashboardCompletoDto
{
    public List<KpiCardDto> KpisPrincipales { get; set; } = new();
    public GraficoDto VacantesPorEstado { get; set; } = new();
    public GraficoDto TopSkillsDemandadas { get; set; } = new();
    public GraficoMultiserieDto InventarioTalento { get; set; } = new();
}

// Para las tarjetas de números grandes (Ej: "15 Vacantes")
public class KpiCardDto
{
    public string Titulo { get; set; } = null!;
    public int Valor { get; set; }
    public string? Icono { get; set; } // Ej: "users", "briefcase"
    public string Color { get; set; } = "primary"; // Ej: "blue", "red"
}

// Para gráficos simples (Barras, Donas)
public class GraficoDto
{
    public List<string> Etiquetas { get; set; } = new(); // Eje X
    public List<int> Valores { get; set; } = new();      // Eje Y
}

// Para gráficos complejos (Barras apiladas: Nivel vs Skill)
public class GraficoMultiserieDto
{
    public List<string> Categorias { get; set; } = new(); // Eje X (Las Skills)
    public List<SerieDatosDto> Series { get; set; } = new(); // Las barras (Niveles)
}

public class SerieDatosDto
{
    public string Nombre { get; set; } = null!; // Ej: "Avanzado"
    public List<int> Datos { get; set; } = new(); // [5, 2, 8...]
}