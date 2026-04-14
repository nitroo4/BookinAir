namespace BOOKINGAPI.Models;
public class OtpCode
{
    public string? Email { get; set; }
    public string? Code { get; set; }
    public DateTime Expiration { get; set; }
}