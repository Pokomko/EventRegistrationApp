using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class ParticipantService : IParticipantService
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IMapper _mapper;

    public ParticipantService(IParticipantRepository participantRepository, IMapper mapper)
    {
        _participantRepository = participantRepository;
        _mapper = mapper;
    }

    public async Task CreatParticipantAsync(ParticipantDto participantDto)
    {
        var participant = _mapper.Map<Participant>(participantDto);

        await _participantRepository.CreateParticipantAsync(participant);
    }

    public async Task<ParticipantDto?> GetParticipantByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ParticipantDto?> GetParticipantByUserIdAsync(Guid userId)
    {
        var participant = await _participantRepository.GetParticipantByUserIdAsync(userId);

        return _mapper.Map<ParticipantDto>(participant);
    }

    public async Task UpdateParticipantAsync(ParticipantDto updatedParticipantDto)
    {
        var existingParticipant = await _participantRepository.GetParticipantByIdAsync(updatedParticipantDto.Id);

        if (existingParticipant == null)
        {
            throw new KeyNotFoundException($"Мероприятие с Id {updatedParticipantDto.Id} не найден.");
        }

        _mapper.Map(updatedParticipantDto, existingParticipant);
        await _participantRepository.UpdateParticipantAsync(existingParticipant);
    }
}
