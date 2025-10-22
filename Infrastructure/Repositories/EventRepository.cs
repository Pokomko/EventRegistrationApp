using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Context;
using Application.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;

    public EventRepository(AppDbContext context, ILogger<EventRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Event>> GetAllEventsAsync()
    {
        try
        {
            var events = await _context.Events
                .Include(e => e.ParticipantEvents)
                .ThenInclude(pe => pe.Participant)
                .ToListAsync();
            return events;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Event?> GetEventByIdAsync(Guid eventId)
    {
        return await _context.Events
               .Include(e => e.ParticipantEvents)
               .ThenInclude(pe => pe.Participant)
               .FirstOrDefaultAsync(e => e.Id == eventId);
    }
    public async Task CreateEventAsync(Event newEvent)
    {
        try
        { 
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<bool> DeleteEventAsync(Event eventToDelete)
    {
        try
        {
            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex) 
        {
            throw;
        }
    }

    public async Task EditEventAsync(Event updatedEvent)
    {
        try {
            _context.Events.Update(updatedEvent);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex) {
            throw;
        }

    }
}
