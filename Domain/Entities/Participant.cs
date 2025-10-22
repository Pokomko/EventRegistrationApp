namespace Domain.Entities;

public class Participant
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public ICollection<ParticipantEvent> ParticipantEvents { get; set; } = new List<ParticipantEvent>();
}
