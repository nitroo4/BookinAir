using System.Net;
using System.Net.Mail;
using BOOKINGAPI.Models;

public class OtpService
{
    private static List<OtpCode> _otps = new();

    public string GenerateOtp(string email)
    {
        var code = new Random().Next(100000, 999999).ToString();

        _otps.RemoveAll(x => x.Email == email);

        _otps.Add(new OtpCode
        {
            Email = email,
            Code = code,
            Expiration = DateTime.UtcNow.AddMinutes(5)
        });

        return code;
    }

    public bool VerifyOtp(string email, string code)
    {
        var otp = _otps.FirstOrDefault(x => x.Email == email);

        if (otp == null) return false;
        if (otp.Expiration < DateTime.UtcNow) return false;

        return otp.Code == code;
    }

    //ENVOI EMAIL GMAIL
    public void SendEmail(string toEmail, string code)
    {
        var fromEmail = "mickarzk@gmail.com";
        var password = "gzds uajd nkid hapd"; // mdp gmail pour stmp

        var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(fromEmail, password),
            EnableSsl = true
        };

        var message = new MailMessage(fromEmail, toEmail)
        {
            Subject = "Votre code OTP",
            Body = $"Votre code OTP est : {code}"
        };

        smtp.Send(message);
    }
}