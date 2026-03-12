using Microsoft.AspNetCore.Mvc;
using BOOKINGAPI.Models;
using BOOKINGAPI.Services;

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
        return await _service.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Billet>> GetById(string id)
    {
        var billet = await _service.GetByIdAsync(id);

        if (billet is null)
            return NotFound();

        return billet;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Billet billet)
    {
        await _service.CreateAsync(billet);
        return CreatedAtAction(nameof(GetById), new { id = billet.Id }, billet);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Billet billet)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing is null)
            return NotFound();

        billet.Id = id;
        await _service.UpdateAsync(id, billet);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing is null)
            return NotFound();

        await _service.DeleteAsync(id);
        return NoContent();
    }
}