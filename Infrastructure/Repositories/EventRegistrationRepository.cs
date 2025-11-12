using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EventRegistrationRepository : IEventRegistrationRepository
{
    private readonly AppDbContext _context;

    public EventRegistrationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ParticipantEvent?> GetRegistrationAsync(Guid participantId, Guid eventId)
    {
        return await _context.ParticipantEvents
            .FirstOrDefaultAsync(pe => pe.ParticipantId == participantId && pe.EventId == eventId);
    }

    public async Task<int> GetEventParticipantCountAsync(Guid eventId)
    {
        return await _context.ParticipantEvents
            .CountAsync(pe => pe.EventId == eventId);
    }

    public async Task RegisterParticipantAsync(ParticipantEvent participantEvent)
    {
        _context.ParticipantEvents.Add(participantEvent);
        await _context.SaveChangesAsync();
    }

    public async Task UnregisterParticipantAsync(ParticipantEvent participantEvent)
    {
        _context.ParticipantEvents.Remove(participantEvent);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsParticipantRegisteredAsync(Guid participantId, Guid eventId)
    {
        return await _context.ParticipantEvents
            .AnyAsync(pe => pe.ParticipantId == participantId && pe.EventId == eventId);
    }
}

