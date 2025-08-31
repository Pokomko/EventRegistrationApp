using Domain.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly IEventRepository _eventRepository;
    //private readonly AppDbContext _context;
    public EventsController(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAll()
    {
        var events = await _eventRepository.GetAllEventsAsync();
        //var events = await _context.Events.ToListAsync();
        return Ok(events);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Create(Event newEvent)
    {
        await _eventRepository.CreateEventAsync(newEvent);
        //_context.Events.Add(newEvent);
        //await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = newEvent.Id }, newEvent);
    }

    [HttpPut]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Update(Event updatedEvent)
    {
        await _eventRepository.EditEventAsync(updatedEvent);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var isDeleted = await _eventRepository.DeleteEventAsync(id);
        if (!isDeleted) {
            return NotFound();
        }
        return NoContent();
    }
}
