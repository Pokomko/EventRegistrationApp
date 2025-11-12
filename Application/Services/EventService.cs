using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public EventService(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<List<EventDto>> GetAllEventsAsync()
    {
        var events = await _eventRepository.GetAllEventsAsync();

        // map entities to DTOs here
        return events.Select(e => new EventDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            StartDateTime = e.StartDateTime,
            Location = e.Location,
            Category = e.Category,
            MaxParticipants = e.MaxParticipants,
            ImageUrl = e.ImageUrl,
            Participants = e.ParticipantEvents?.Select(pe => new EventParticipantDto
            {
                EventId = pe.EventId,
                Participant = new ParticipantDto
                {
                    Id = pe.Participant.Id,
                    FirstName = pe.Participant.FirstName,
                    LastName = pe.Participant.LastName,
                    BirthDate = pe.Participant.BirthDate
                },
                UserId = pe.Participant.UserId,
                RegisteredAt = pe.RegisteredAt
            }).ToList() ?? new List<EventParticipantDto>()
        }).ToList();
    }
    public async Task<EventDto> GetEventByIdAsync(Guid eventId)
    {
        var e = await _eventRepository.GetEventByIdAsync(eventId);
        if (e == null) return null; // will cause nullable warning, but controller can handle

        return new EventDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            StartDateTime = e.StartDateTime,
            Location = e.Location,
            Category = e.Category,
            MaxParticipants = e.MaxParticipants,
            ImageUrl = e.ImageUrl,
            Participants = e.ParticipantEvents?.Select(pe => new EventParticipantDto
            {
                EventId = pe.EventId,
                Participant = new ParticipantDto
                {
                    Id = pe.Participant.Id,
                    FirstName = pe.Participant.FirstName,
                    LastName = pe.Participant.LastName,
                    BirthDate = pe.Participant.BirthDate
                },
                UserId = pe.Participant?.UserId ?? Guid.Empty,
                RegisteredAt = pe.RegisteredAt
            }).ToList() ?? new List<EventParticipantDto>()
        };
    }
    public async Task<(List<EventDto>, int totalCount)> GetPagedEventsAsync(int page, int pageSize, string? queryString = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var (pagedEvents, totalCount) = await _eventRepository.GetPagedEventsAsync(page, pageSize, queryString, startDate, endDate);

        var eventDtos = pagedEvents.Select(e => new EventDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            StartDateTime = e.StartDateTime,
            Location = e.Location,
            Category = e.Category,
            MaxParticipants = e.MaxParticipants,
            ImageUrl = e.ImageUrl,
            Participants = e.ParticipantEvents?.Select(pe => new EventParticipantDto
            {
                EventId = pe.EventId,
                Participant = new ParticipantDto
                {
                    Id = pe.Participant.Id,
                    FirstName = pe.Participant.FirstName,
                    LastName = pe.Participant.LastName,
                    BirthDate = pe.Participant.BirthDate
                },
                UserId = pe.Participant.UserId,
                RegisteredAt = pe.RegisteredAt
            }).ToList() ?? new List<EventParticipantDto>()
        }).ToList();

        return (eventDtos, totalCount);
    }
    public async Task CreateEventAsync(CreateEventDto newEventDto)
    {
        var newEvent = _mapper.Map<Event>(newEventDto);

        await _eventRepository.CreateEventAsync(newEvent);
    }

    public async Task EditEventAsync(UpdateEventDto updatedEventDto)
    {
        var existingEvent = await _eventRepository.GetEventByIdAsync(updatedEventDto.Id);

        if (existingEvent == null)
        {
            throw new KeyNotFoundException($"Мероприятие с Id {updatedEventDto.Id} не найден.");
        }

        if (existingEvent.MaxParticipants > updatedEventDto.MaxParticipants)
        {
            throw new InvalidOperationException("Нельзя уменьшить количество участников ниже текущего значения.");
        }

        _mapper.Map(updatedEventDto, existingEvent);

        await _eventRepository.EditEventAsync(existingEvent);
    }

    public async Task<bool> DeleteEventAsync(Guid eventId)
    {
        var eventToDelete = await _eventRepository.GetEventByIdAsync(eventId);
        if (eventToDelete != null)
        {
            return await _eventRepository.DeleteEventAsync(eventToDelete);
        }

        return false;
    }

}
