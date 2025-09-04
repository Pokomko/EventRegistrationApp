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
    private readonly IEventService _eventService;
    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAll()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Create(Event newEvent)
    {
        await _eventService.CreateEventAsync(newEvent);
        return CreatedAtAction(nameof(GetAll), new { id = newEvent.Id }, newEvent);
    }

    [HttpPut]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Update(Event updatedEvent)
    {
        await _eventService.EditEventAsync(updatedEvent);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var isDeleted = await _eventService.DeleteEventAsync(id);
        if (!isDeleted) {
            return NotFound();
        }
        return NoContent();
    }
}
