using Domain.Entities;

namespace Application.Interfaces;

public interface IEventRepository
{
    Task<List<Event>> GetAllEventsAsync();
    Task CreateEventAsync(Event newEvent);

    Task EditEventAsync(Event updatedEvent);
    Task<bool> DeleteEventAsync(Guid eventId);
}
