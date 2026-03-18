using Microsoft.AspNetCore.Mvc;
using BOOKINGAPI.DTos;
using BOOKINGAPI.Models;
using BOOKINGAPI.Services;
using BCrypt.Net;

namespace BOOKINGAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var existingUser = await _userService.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            return BadRequest(new { message = "Cet email existe déjà." });

        var user = new User
        {
            Nom = dto.Nom,
            Email = dto.Email,
            Adresse = dto.Adresse,
            Numero = dto.Numero,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _userService.CreateAsync(user);

        return Ok(new { message = "Inscription réussie." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userService.GetByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized(new { message = "Email ou mot de passe invalide." });

        bool passwordOk = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        if (!passwordOk)
            return Unauthorized(new { message = "Email ou mot de passe invalide." });

        var response = new AuthResponseDto
        {
            Id = user.Id,
            Nom = user.Nom,
            Email = user.Email,
            Role = user.Role.ToString()
        };

        return Ok(response);
    }
}