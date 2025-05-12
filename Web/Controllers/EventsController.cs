using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _context;
    public EventsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAll()
    {
        var events = await _context.Events.ToListAsync();
        return Ok(events);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Create(Event newEvent)
    {
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = newEvent.Id }, newEvent);
    }

    [HttpPut]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Update(Event newEvent)
    {
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var eventToDelete = await _context.Events.FindAsync(id);

        if (eventToDelete == null)
        {
            return NotFound();
        }

        _context.Events.Remove(eventToDelete);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
