using Domain.Entities;
using Application.DTO;

namespace Application.Interfaces;

public interface IEventRepository
{
    Task<List<Event>> GetAllEventsAsync();
    Task<Event?> GetEventByIdAsync(Guid eventId);
    Task CreateEventAsync(Event newEvent);
    Task EditEventAsync(Event updatedEvent);
    Task<bool> DeleteEventAsync(Event eventToDelete);
}
