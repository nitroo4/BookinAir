namespace BOOKINGAPI.DTos;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string? Id { get; set; }
    public string Nom { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
}