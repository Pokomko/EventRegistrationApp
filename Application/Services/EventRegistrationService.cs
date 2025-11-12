using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class EventRegistrationService : IEventRegistrationService
{
    private readonly IEventRegistrationRepository _registrationRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly IEventRepository _eventRepository;

    public EventRegistrationService(
        IEventRegistrationRepository registrationRepository,
        IParticipantRepository participantRepository,
        IEventRepository eventRepository)
    {
        _registrationRepository = registrationRepository;
        _participantRepository = participantRepository;
        _eventRepository = eventRepository;
    }

    public async Task RegisterAsync(Guid userId, Guid eventId)
    {
        // Получаем участника по userId
        var participant = await _participantRepository.GetParticipantByUserIdAsync(userId);
        if (participant == null)
        {
            throw new InvalidOperationException("Participant not found for the current user");
        }

        // Получаем событие
        var eventEntity = await _eventRepository.GetEventByIdAsync(eventId);
        if (eventEntity == null)
        {
            throw new InvalidOperationException($"Event with id {eventId} not found");
        }

        // Проверяем, не зарегистрирован ли уже участник
        var isRegistered = await _registrationRepository.IsParticipantRegisteredAsync(participant.Id, eventId);
        if (isRegistered)
        {
            throw new InvalidOperationException("User is already registered for this event");
        }

        // Проверяем лимит участников
        var currentCount = await _registrationRepository.GetEventParticipantCountAsync(eventId);
        if (currentCount >= eventEntity.MaxParticipants)
        {
            throw new InvalidOperationException("Event is full. No available spots.");
        }

        // Создаем регистрацию
        var participantEvent = new ParticipantEvent
        {
            ParticipantId = participant.Id,
            EventId = eventId,
            RegisteredAt = DateTime.UtcNow,
            Participant = participant,
            Event = eventEntity
        };

        await _registrationRepository.RegisterParticipantAsync(participantEvent);
    }

    public async Task UnregisterAsync(Guid userId, Guid eventId)
    {
        // Получаем участника по userId
        var participant = await _participantRepository.GetParticipantByUserIdAsync(userId);
        if (participant == null)
        {
            throw new InvalidOperationException("Participant not found for the current user");
        }

        // Получаем событие
        var eventEntity = await _eventRepository.GetEventByIdAsync(eventId);
        if (eventEntity == null)
        {
            throw new InvalidOperationException($"Event with id {eventId} not found");
        }

        // Проверяем, зарегистрирован ли участник
        var registration = await _registrationRepository.GetRegistrationAsync(participant.Id, eventId);
        if (registration == null)
        {
            throw new InvalidOperationException("User is not registered for this event");
        }

        // Удаляем регистрацию
        await _registrationRepository.UnregisterParticipantAsync(registration);
    }
}
