using Application.DTO;
using Application.Interfaces;
using Azure;
using Domain.Entities;
using Infrastructure.Context;
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
                .OrderBy(e => e.StartDateTime)
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
    public async Task<(List<Event>, int TotalCount)> GetPagedEventsAsync(int page, int pageSize, string? queryString = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Events
            .AsQueryable();

        if (!string.IsNullOrEmpty(queryString))
        {
            query = query.Where(e =>
            e.Title.Contains(queryString) ||
            e.Description.Contains(queryString) ||
            e.Category.Contains(queryString)
            );
        }

        if (startDate.HasValue) {
            query = query.Where(e => e.StartDateTime >= startDate.Value.Date);
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.StartDateTime <= endDate.Value.Date);
        }

        var totalCount = query.Count();

        var events = await query
            .Include(e => e.ParticipantEvents)
            .ThenInclude(pe => pe.Participant)
            .OrderBy(e => e.StartDateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (events, totalCount);
    }

    public async Task<bool> DeleteEventAsync(Event eventToDelete)
    {
        try
        {
            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
        catch {
            throw;
        }
    }

    public async Task EditEventAsync(Event updatedEvent)
    {
        try {
            _context.Events.Update(updatedEvent);
            await _context.SaveChangesAsync();
        }
        catch {
            throw;
        }
    }
}
