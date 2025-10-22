namespace Application.DTO;

public class EventParticipantDto
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public ParticipantDto Participant { get; set; } = new ParticipantDto();
    public DateTime RegisteredAt { get; set; }
}
