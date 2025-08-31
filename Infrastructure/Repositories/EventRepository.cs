using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            var events = await _context.Events.ToListAsync();
            return events;
        }
        catch (Exception ex)
        {
            throw;
        }
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

    public async Task<bool> DeleteEventAsync(Guid eventId)
    {
        try
        {
            var eventToDelete = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            if (eventToDelete != null) { 
                _context.Events.Remove(eventToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (Exception ex) 
        {
            throw;
        }
    }

    public Task EditEventAsync(Event updatedEvent)
    {
        throw new NotImplementedException();
    }
}
