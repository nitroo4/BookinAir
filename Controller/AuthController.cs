using Microsoft.AspNetCore.Mvc;
using BOOKINGAPI.DTos;
using BOOKINGAPI.Models;
using BOOKINGAPI.Services;
using BOOKINGAPI.Enums;

namespace BOOKINGAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly JwtService _jwtService;
    private readonly OtpService _otpService;

    public AuthController(UserService userService, JwtService jwtService, OtpService otpService)
    {
        _userService = userService;
        _jwtService = jwtService;
        _otpService = otpService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        dto.Email = dto.Email.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(dto.Nom) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest(new { message = "Nom, email et mot de passe sont obligatoires." });
        }

        var existingUser = await _userService.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return BadRequest(new { message = "Cet email existe déjà." });
        }

        var user = new User
        {
            Nom = dto.Nom,
            Email = dto.Email,
            Adresse = dto.Adresse,
            Numero = dto.Numero,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Client
        };

        await _userService.CreateAsync(user);

        return Ok(new { message = "Inscription réussie." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest(new { message = "Email et mot de passe sont obligatoires." });
        }

        dto.Email = dto.Email.Trim().ToLower();

        var user = await _userService.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            return Unauthorized(new { message = "Email ou mot de passe invalide." });
        }

        bool passwordOk = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        if (!passwordOk)
        {
            return Unauthorized(new { message = "Email ou mot de passe invalide." });
        }

        // 🔥 OTP UNIQUEMENT
        var code = _otpService.GenerateOtp(user.Email);
        _otpService.SendEmail(user.Email, code);

        return Ok(new { message = "OTP envoyé à votre email." });
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
    {
        var valid = _otpService.VerifyOtp(dto.Email, dto.Code);

        if (!valid)
            return Unauthorized(new { message = "OTP invalide" });

        var user = await _userService.GetByEmailAsync(dto.Email);

        // ✅ connexion réelle ici
        user.Status = UserStatus.Connecte;
        await _userService.UpdateAsync(user.Id!, user);

        var token = _jwtService.GenerateToken(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Id = user.Id,
            Nom = user.Nom,
            Email = user.Email,
            Role = user.Role.ToString()
        });
    }

    [HttpPost("logout/{id}")]
    public async Task<IActionResult> Logout(string id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        user.Status = UserStatus.Deconnecte;
        await _userService.UpdateAsync(user.Id!, user);

        return Ok(new { message = "Utilisateur déconnecté." });
    }
}