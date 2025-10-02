using Domain.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTO;

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
        var eventList = await _eventService.GetAllEventsAsync();
        return Ok(eventList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Event>>> GetById(Guid id)
    {
        var eventItem = await _eventService.GetEventByIdAsync(id);
        return Ok(eventItem);
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
    public async Task<IActionResult> Update(UpdateEventDto updatedEvent)
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
