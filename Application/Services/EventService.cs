using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class EventService : IEventService
{
    readonly IEventRepository _eventRepository;
    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<List<Event>> GetAllEventsAsync()
    {
        return await _eventRepository.GetAllEventsAsync();
    }

    public async Task CreateEventAsync(Event newEvent)
    {
        await _eventRepository.CreateEventAsync(newEvent);
    }

    public async Task EditEventAsync(Event updatedEvent)
    {
        await _eventRepository.EditEventAsync(updatedEvent);
    }

    public async Task<bool> DeleteEventAsync(Guid eventId)
    {
        return await _eventRepository.DeleteEventAsync(eventId);
    }
}
