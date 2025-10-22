using Domain.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTO;
using System.Linq;

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
    public async Task<ActionResult<IEnumerable<EventDto>>> GetAll()
    {
        var dtoList = await _eventService.GetAllEventsAsync();
        return Ok(dtoList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetById(Guid id)
    {
        var eventItem = await _eventService.GetEventByIdAsync(id);
        if (eventItem == null) return NotFound();
        return Ok(eventItem);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Create(CreateEventDto newEventDto)
    {
        await _eventService.CreateEventAsync(newEventDto);
        return Ok();
    }

    [HttpPut]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Update(UpdateEventDto updatedEventDto)
    {
        await _eventService.EditEventAsync(updatedEventDto);
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
