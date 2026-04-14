using Microsoft.AspNetCore.Mvc;
using BOOKINGAPI.Models;
using BOOKINGAPI.Services;
using BOOKINGAPI.DTos;

namespace BOOKINGAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservaController : ControllerBase
{
    private readonly ReservaService _service;

    public ReservaController(ReservaService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Create([FromBody] ReservaDto dto)
    {
        if (dto == null)
            return BadRequest("DTO is null");

        var reserva = new Reserva
        {
            User_Id = dto.User_Id,
            Destination_Id = dto.Destination_Id,
            Qty = dto.Qty,
            Prix = dto.Prix
        };

        _service.Create(reserva);

        return Ok(new { message = "Reservation created" });
    }
}