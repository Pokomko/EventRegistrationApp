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
            var events = await _context.Events.ToListAsync();
            return events;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Event> GetEventByIdAsync(Guid eventId)
    {
        try
        {
            return await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
        }
        catch (Exception ex) {
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

    public async Task EditEventAsync(UpdateEventDto updatedEvent)
    {
        try {
            var existingEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == updatedEvent.Id);

            if (existingEvent == null)
            {
                throw new KeyNotFoundException($"Event with Id {updatedEvent.Id} not found.");
            }

            existingEvent.Title = updatedEvent.Title;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.StartDateTime = updatedEvent.StartDateTime;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.Category = updatedEvent.Category;
            existingEvent.MaxParticipants = updatedEvent.MaxParticipants;

            // check existingEvent.MaxParticipants > updatedEvent.MaxParticipants; !!!

            _context.Events.Update(existingEvent);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex) {
            throw;
        }

    }
}
