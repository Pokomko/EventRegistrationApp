using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Application.Services;

public class EventService : IEventService
{
    readonly IEventRepository _eventRepository;
    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
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
                    ParticipantId = pe.Participant.Id,
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
                    ParticipantId = pe.Participant.Id,
                    FirstName = pe.Participant.FirstName,
                    LastName = pe.Participant.LastName,
                    BirthDate = pe.Participant.BirthDate
                },
                UserId = pe.Participant?.UserId ?? Guid.Empty,
                RegisteredAt = pe.RegisteredAt
            }).ToList() ?? new List<EventParticipantDto>()
        };
    }
    public async Task<(List<EventDto>, int totalCount)> GetPagedEventsAsync(int page, int pageSize)
    {
        var (pagedEvents, totalCount) = await _eventRepository.GetPagedEventsAsync(page, pageSize);

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
                    ParticipantId = pe.Participant.Id,
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
        var newEvent = new Event
        {
            Id = Guid.NewGuid(),
            Title = newEventDto.Title,
            Description = newEventDto.Description,
            StartDateTime = newEventDto.StartDateTime,
            Location = newEventDto.Location,
            Category = newEventDto.Category,
            MaxParticipants = newEventDto.MaxParticipants,
            ImageUrl = newEventDto.ImageUrl,
        };

        await _eventRepository.CreateEventAsync(newEvent);
    }

    public async Task EditEventAsync(UpdateEventDto updatedEvent)
    {
        var existingEvent = await _eventRepository.GetEventByIdAsync(updatedEvent.Id);

        if (existingEvent == null)
        {
            throw new KeyNotFoundException($"Event with Id {updatedEvent.Id} not found.");
        }

        existingEvent.Title = updatedEvent.Title;
        existingEvent.Description = updatedEvent.Description;
        existingEvent.StartDateTime = updatedEvent.StartDateTime;
        existingEvent.Location = updatedEvent.Location;
        existingEvent.Category = updatedEvent.Category;
        existingEvent.MaxParticipants = updatedEvent.MaxParticipants;
        existingEvent.ImageUrl = updatedEvent.ImageUrl;

        // check existingEvent.MaxParticipants > updatedEvent.MaxParticipants; !!!

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
