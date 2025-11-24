using System.ComponentModel.DataAnnotations;

namespace TalentoInterno.CORE.Core.DTOs;

public class VacanteCreateDTO
{
    [Required]
    public string Titulo { get; set; } = null!;
    public int PerfilId { get; set; }
    public int? CuentaId { get; set; }
    public int? ProyectoId { get; set; }
    public DateOnly FechaInicio { get; set; }
    public int UrgenciaId { get; set; }
    public string Estado { get; set; }
    public string Descripcion { get; set; } = "";

    // HU-06: Aquí se reciben las skills y sus pesos/niveles
    public List<VacanteSkillReqCreateDTO>? Skills { get; set; }
}