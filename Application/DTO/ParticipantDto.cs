namespace Application.DTO
{
    public class ParticipantDto
    {
        public Guid ParticipantId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}
