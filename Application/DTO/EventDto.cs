namespace Application.DTO;

public class EventDto : EventBaseDto
{
    public Guid Id { get; set; }

    // Participants in the event
    public List<EventParticipantDto> Participants { get; set; } = new List<EventParticipantDto>();
}
