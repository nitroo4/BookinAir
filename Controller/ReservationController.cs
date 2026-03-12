using Microsoft.AspNetCore.Mvc;
using BOOKINGAPI.Models;
using BOOKINGAPI.Services;

namespace BOOKINGAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _service;

    public ReservationsController(ReservationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Reservation>>> GetAll()
    {
        var reservations = await _service.GetAllAsync();
        return Ok(reservations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Reservation>> GetById(string id)
    {
        var reservation = await _service.GetByIdAsync(id);

        if (reservation is null)
            return NotFound("Réservation introuvable.");

        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Reservation reservation)
    {
        var userExists = await _service.UserExistsAsync(reservation.UserId);
        if (!userExists)
            return BadRequest("L'utilisateur n'existe pas.");

        var destinationExists = await _service.DestinationExistsAsync(reservation.Id_Destination);
        if (!destinationExists)
            return BadRequest("La destination n'existe pas.");

        await _service.CreateAsync(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Reservation reservation)
    {
        var existingReservation = await _service.GetByIdAsync(id);
        if (existingReservation is null)
            return NotFound("Réservation introuvable.");

        var userExists = await _service.UserExistsAsync(reservation.UserId);
        if (!userExists)
            return BadRequest("L'utilisateur n'existe pas.");

        var destinationExists = await _service.DestinationExistsAsync(reservation.Id_Destination);
        if (!destinationExists)
            return BadRequest("La destination n'existe pas.");

        reservation.Id = id;
        await _service.UpdateAsync(id, reservation);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existingReservation = await _service.GetByIdAsync(id);
        if (existingReservation is null)
            return NotFound("Réservation introuvable.");

        await _service.DeleteAsync(id);
        return NoContent();
    }
}