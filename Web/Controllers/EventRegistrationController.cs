using System;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EventRegistrationController : ControllerBase
{
    private readonly IEventRegistrationService _eventRegistrationService;

    public EventRegistrationController(IEventRegistrationService eventRegistrationService)
    {
        _eventRegistrationService = eventRegistrationService;
    }

    // POST api/EventRegistration/{eventId}
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] SubscribeToEventDto eventDto)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        await _eventRegistrationService.RegisterAsync(userId, eventDto.eventId);
        return Ok();
    }

    [HttpDelete("{eventId}")]
    public async Task<IActionResult> UnRegister(Guid eventId)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        await _eventRegistrationService.UnregisterAsync(userId, eventId);

        return NoContent();
    }
}
