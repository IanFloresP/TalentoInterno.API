namespace TalentoInterno.CORE.Core.DTOs;

public class RolDto
{
    // Dejamos el RolId aquí para que sea útil al crear
    // (aunque tu POST no lo usa) y al leer (GET).
    public int RolId { get; set; }

    public string Nombre { get; set; } = null!;
}