namespace Domain.Entities
{
    public class ParticipantEvent
    {
        public Guid EventId { get; set; }
        public Event Event { get; set; } = null!;

        public Guid ParticipantId { get; set; }
        public Participant Participant { get; set; } = null!;

        public DateTime RegisteredAt { get; set; }
    }
}
