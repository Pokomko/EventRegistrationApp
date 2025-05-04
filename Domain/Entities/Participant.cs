namespace Domain.Entities;

public class Participant
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string Email { get; set; } = null!;

    public ICollection<ParticipantEvent> ParticipantEvents { get; set; } = new List<ParticipantEvent>();
}
