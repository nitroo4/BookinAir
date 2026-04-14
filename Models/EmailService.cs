using System.Net;
using System.Net.Mail;

public class EmailService
{
    public void SendOtp(string toEmail, string otp)
    {
        var from = "tonemail@gmail.com";
        var password = "mot_de_passe_application"; // ⚠️ PAS ton vrai mdp

        var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(from, password),
            EnableSsl = true
        };

        var message = new MailMessage(from, toEmail)
        {
            Subject = "Votre code OTP",
            Body = $"Votre code est : {otp}"
        };

        smtp.Send(message);
    }
}