namespace TalentoInterno.CORE.Core.DTOs;

public class ArchivoDTO
{
    public string ContentType { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public byte[] Data { get; set; } = null!;
}