using Domain.Entities;

namespace Application.Interfaces;

public interface IEventRegistrationRepository
{
    Task<ParticipantEvent?> GetRegistrationAsync(Guid participantId, Guid eventId);
    Task<int> GetEventParticipantCountAsync(Guid eventId);
    Task RegisterParticipantAsync(ParticipantEvent participantEvent);
    Task UnregisterParticipantAsync(ParticipantEvent participantEvent);
    Task<bool> IsParticipantRegisteredAsync(Guid participantId, Guid eventId);
}

