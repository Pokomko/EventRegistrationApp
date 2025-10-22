namespace Application.Interfaces
{
    public interface IEventRegistrationService
    {
        Task RegisterAsync(Guid userId, Guid eventId);
    }
}
