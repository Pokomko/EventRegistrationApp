namespace Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDateTime { get; set; }
    public string Location { get; set; } = null!;
    public string Category { get; set; } = null!;
    public int MaxParticipants { get; set; }
    public string? ImageUrl { get; set; }

    public ICollection<ParticipantEvent> ParticipantEvents { get; set; } = new List<ParticipantEvent>();
}
