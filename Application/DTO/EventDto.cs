namespace Application.DTO;

public class EventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public string? ImageUrl { get; set; }

    // Participants in the event
    public List<EventParticipantDto> Participants { get; set; } = new List<EventParticipantDto>();
}
