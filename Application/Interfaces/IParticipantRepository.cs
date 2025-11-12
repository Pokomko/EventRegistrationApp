using Domain.Entities;

namespace Application.Interfaces;

public interface IParticipantRepository
{
    Task<Participant?> GetParticipantByUserIdAsync(Guid userId);
    Task<Participant?> GetParticipantByIdAsync(Guid id);
    Task UpdateParticipantAsync(Participant participant);
    Task CreateParticipantAsync(Participant participant);
}
