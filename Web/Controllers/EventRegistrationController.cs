using System;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EventRegistrationController : ControllerBase
{
    private readonly AppDbContext _context;

    public EventRegistrationController(AppDbContext context)
    {
        _context = context;
    }

    // POST api/EventRegistration/{eventId}
    [HttpPost("{eventId}")]
    public async Task<IActionResult> Register(Guid eventId)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var evt = await _context.Events
            .Include(e => e.ParticipantEvents)
            .FirstOrDefaultAsync(e => e.Id == eventId);

        if (evt == null)
        {
            return NotFound("Event not found");
        }

        // Ensure participant exists
        var participant = await _context.Participants
            .Include(p => p.ParticipantEvents)
            .FirstOrDefaultAsync(p => p.UserId == userId || p.Id == userId);

        if (participant == null)
        {
            throw new InvalidOperationException("Participant not found");
        }

        // Check already registered
        var existing = await _context.ParticipantEvents.FindAsync(participant.Id, evt.Id);
        if (existing != null)
        {
            return Conflict("User already registered for this event");
        }

        // Check capacity
        var currentCount = await _context.ParticipantEvents.CountAsync(pe => pe.EventId == evt.Id);
        if (currentCount >= evt.MaxParticipants)
        {
            return BadRequest("Event is full");
        }

        var participantEvent = new ParticipantEvent
        {
            ParticipantId = participant.Id,
            EventId = evt.Id,
            RegisteredAt = DateTime.UtcNow,
            Participant = participant,
            Event = evt
        };

        _context.ParticipantEvents.Add(participantEvent);

        await _context.SaveChangesAsync();

        return Ok();
    }
}
