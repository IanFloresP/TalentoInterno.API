namespace TalentoInterno.CORE.Core.DTOs;

public class LoginResponseDTO
{
    public string AccessToken { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public string Role { get; set; } = null!;
    public int ColaboradorId { get; set; }
    public string RefreshToken { get; set; } = null!;
}