using Domain.Entities;
using Application.DTO;

namespace Application.Interfaces;

public interface IEventService
{
    Task<List<EventDto>> GetAllEventsAsync();
    Task<EventDto> GetEventByIdAsync(Guid eventId);
    Task CreateEventAsync(CreateEventDto newEvent);
    Task EditEventAsync(UpdateEventDto updatedEvent);
    Task<bool> DeleteEventAsync(Guid eventId);
}
