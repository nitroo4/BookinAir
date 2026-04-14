using Microsoft.AspNetCore.Mvc;
using BOOKINGAPI.Models;
using BOOKINGAPI.Services;
using BOOKINGAPI.DTOs;

namespace BOOKINGAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BilletsController : ControllerBase
{
    private readonly BilletService _service;

    public BilletsController(BilletService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Billet>>> GetAll()
    {
        var billets = await _service.GetAllAsync();
        return Ok(billets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Billet>> GetById(string id)
    {
        var billet = await _service.GetByIdAsync(id);

        if (billet is null)
            return NotFound("Billet introuvable.");

        return Ok(billet);
    }

    [HttpGet("avec-destination")]
    public async Task<ActionResult<List<BilletAvecDestinationDto>>> GetAllAvecDestination()
    {
        var billets = await _service.GetAllBilletsAvecDestinationAsync();
        return Ok(billets);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Billet billet)
    {
        var exists = await _service.ExistsByNumBilletAsync(billet.Num_Billet);

        if (exists)
        {
            return BadRequest("Ce numéro de billet existe déjà.");
        }
        await _service.CreateAsync(billet);
        return CreatedAtAction(nameof(GetById), new { id = billet.Id }, billet);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Billet billet)
    {
        var existing = await _service.GetByIdAsync(id);

        if (existing is null)
            return NotFound("Billet introuvable.");

        billet.Id = id;
        await _service.UpdateAsync(id, billet);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existing = await _service.GetByIdAsync(id);

        if (existing is null)
            return NotFound("Billet introuvable.");

        await _service.DeleteAsync(id);
        return NoContent();
    }
}