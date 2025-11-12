using Application.DTO;

namespace Application.Interfaces;

public interface IParticipantService
{
    Task<ParticipantDto?> GetParticipantByUserIdAsync(Guid userId);
    Task<ParticipantDto?> GetParticipantByIdAsync(Guid id);
    Task UpdateParticipantAsync(ParticipantDto participantDto);
    Task CreatParticipantAsync(ParticipantDto participantDto);
}
