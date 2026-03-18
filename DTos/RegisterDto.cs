namespace BOOKINGAPI.DTos;

public class RegisterDto
{
    public string Nom { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Adresse { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string Password { get; set; } = null!;
}