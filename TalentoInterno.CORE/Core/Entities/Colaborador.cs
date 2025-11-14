using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // ¡Asegúrate de tener esto!

namespace TalentoInterno.CORE.Core.Entities;

public partial class Colaborador
{
    public int ColaboradorId { get; set; }

    [Required(ErrorMessage = "El campo Nombres es requerido")] // Es buena práctica añadir [Required]
    public string Nombres { get; set; } = null!;

    [Required(ErrorMessage = "El campo Apellidos es requerido")] // Es buena práctica añadir [Required]
    public string Apellidos { get; set; } = null!;

    [Required(ErrorMessage = "El campo Email es requerido")] // Es buena práctica añadir [Required]
    public string Email { get; set; } = null!;

    // --- AQUÍ ESTÁ LA CORRECCIÓN ---
    // [Required] va en la llave foránea (el Id)

    [Required]
    public int RolId { get; set; }
    [Required]
    public int AreaId { get; set; }
    [Required]
    public int? DepartamentoId { get; set; }

    public bool? DisponibleMovilidad { get; set; }

    public bool? Activo { get; set; }

    public DateOnly? FechaAlta { get; set; }


    // --- NAVEGACIÓN (NO TOCAR) ---
    public virtual ICollection<ColaboradorCertificacion> ColaboradorCertificacion { get; set; } = new List<ColaboradorCertificacion>();

    public virtual ICollection<ColaboradorSkill> ColaboradorSkill { get; set; } = new List<ColaboradorSkill>();

    // Ajusta las propiedades de navegación para que sean consistentes
    // Si la llave foránea (DepartamentoId) es nullable (int?), la navegación DEBE ser nullable.
    public virtual Departamento? Departamento { get; set; }

    // Si la llave foránea (RolId) no es nullable (int), la navegación PUEDE ser no-nullable.
    public virtual Rol Rol { get; set; } = null!;

    // Si la llave foránea (AreaId) no es nullable (int), la navegación PUEDE ser no-nullable.
    public virtual Area Area { get; set; } = null!;
}
