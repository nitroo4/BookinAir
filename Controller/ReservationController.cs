using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

    // Admin : peut voir toutes les réservations
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<Reservation>>> GetAll()
    {
        var reservations = await _service.GetAllAsync();
        return Ok(reservations);
    }

    // Client : peut voir uniquement sa propre réservation
    // Admin : peut voir n'importe quelle réservation
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Reservation>> GetById(string id)
    {
        var reservation = await _service.GetByIdAsync(id);

        if (reservation is null)
            return NotFound("Réservation introuvable.");

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

        if (currentUserRole == "Admin" || reservation.UserId == currentUserId)
            return Ok(reservation);

        return Forbid();
    }

    // Client connecté : peut créer une réservation pour lui-même
    // Admin : peut aussi créer
    [Authorize(Roles = "Client,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Reservation reservation)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized("Utilisateur non authentifié.");

        // Si c'est un client, on force son propre UserId
        if (currentUserRole == "Client")
        {
            reservation.UserId = currentUserId;
        }

        var userExists = await _service.UserExistsAsync(reservation.UserId);
        if (!userExists)
            return BadRequest("L'utilisateur n'existe pas.");

        var destinationExists = await _service.DestinationExistsAsync(reservation.Id_Destination);
        if (!destinationExists)
            return BadRequest("La destination n'existe pas.");

        await _service.CreateAsync(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    // Client connecté : peut voir ses propres réservations
    // Admin : peut voir toutes les réservations
    [Authorize]
    [HttpGet("my-reservations")]
    public async Task<ActionResult<List<Reservation>>> GetMyReservations()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized("Utilisateur non authentifié.");

        if (currentUserRole == "Admin")
        {
            var allReservations = await _service.GetAllAsync();
            return Ok(allReservations);
        }

        var myReservations = await _service.GetByUserIdAsync(currentUserId);
        return Ok(myReservations);
    }

    // Client : peut modifier seulement sa propre réservation
    // Admin : peut modifier toutes les réservations
    [Authorize(Roles = "Client,Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Reservation reservation)
    {
        var existingReservation = await _service.GetByIdAsync(id);
        if (existingReservation is null)
            return NotFound("Réservation introuvable.");

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized("Utilisateur non authentifié.");

        // Client : interdit de modifier la réservation d'un autre utilisateur
        if (currentUserRole == "Client" && existingReservation.UserId != currentUserId)
            return Forbid();

        // Si client, on force son propre UserId
        if (currentUserRole == "Client")
        {
            reservation.UserId = currentUserId;
        }

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

    // Client : peut supprimer seulement sa propre réservation
    // Admin : peut supprimer toutes les réservations
    [Authorize(Roles = "Client,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existingReservation = await _service.GetByIdAsync(id);
        if (existingReservation is null)
            return NotFound("Réservation introuvable.");

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized("Utilisateur non authentifié.");

        // Client : interdit de supprimer la réservation d'un autre utilisateur
        if (currentUserRole == "Client" && existingReservation.UserId != currentUserId)
            return Forbid();

        await _service.DeleteAsync(id);
        return NoContent();
    }
}