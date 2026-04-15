using Microsoft.AspNetCore.Mvc;
using BOOKINGAPI.Models;
using BOOKINGAPI.Services;
using BOOKINGAPI.DTos;
using System.Security.Claims;

namespace BOOKINGAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservaController : ControllerBase
{
    private readonly ReservaService _service;
    private readonly BilletService _billetService;

    public ReservaController(ReservaService service,  BilletService billetService)
    {
        _service = service;
        _billetService = billetService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReservaDto dto)
    {
        if (dto == null)
            return BadRequest("DTO is null");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _billetService.AcheterBilletAsync(dto.Billet_Id, dto.Qty);

        if (!success)
            return BadRequest("Stock insuffisant ou billet introuvable");

        var reserva = new Reserva
        {
            User_Id = userId,
            Destination_Id = dto.Destination_Id,
            Billet_Id = dto.Billet_Id,
            Qty = dto.Qty,
            Prix = dto.Prix
        };

        _service.Create(reserva);
        return Ok(new { message = "Reservation created" });
    }

    [HttpGet]
    public async Task<ActionResult<List<Reserva>>> GetAll()
    {
        var reservation = await _service.GetAllAsync();
        return Ok(reservation);
    }
}