using Domain.Entities;

namespace Domain.Abstractions;

public interface IEventRepository1
{
    Task<List<Event>> GetAllEventsAsync();
    Task CreateEventAsync(Event newEvent);

    Task EditEventAsync(Event updatedEvent);
    Task<bool> DeleteEventAsync(Guid eventId);
}
