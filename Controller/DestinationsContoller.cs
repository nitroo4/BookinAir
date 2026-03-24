using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BOOKINGAPI.Models;
using BOOKINGAPI.Services;

namespace BOOKINGAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DestinationsController : ControllerBase
{
    private readonly DestinationService _service;

    public DestinationsController(DestinationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Destination>>> GetAll()
    {
        var destinations = await _service.GetAllAsync();
        return Ok(destinations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Destination>> GetById(string id)
    {
        var destination = await _service.GetByIdAsync(id);

        if (destination is null)
            return NotFound("Destination introuvable.");

        return Ok(destination);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Destination destination)
    {
        await _service.CreateAsync(destination);
        return CreatedAtAction(nameof(GetById), new { id = destination.Id }, destination);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Destination destination)
    {
        var existingDestination = await _service.GetByIdAsync(id);

        if (existingDestination is null)
            return NotFound("Destination introuvable.");

        destination.Id = id;
        await _service.UpdateAsync(id, destination);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existingDestination = await _service.GetByIdAsync(id);

        if (existingDestination is null)
            return NotFound("Destination introuvable.");

        await _service.DeleteAsync(id);
        return NoContent();
    }
}