using Application.Interfaces;
using Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Event;

[Authorize]
public class EventModel : PageModel
{
    public EventDto? Event { get; set; }
    public string UserName { get; set; } = string.Empty;
    public bool IsAdmin => User.IsInRole("Admin");
    public bool IsRegistered { get; set; } = false;

    private readonly IEventService _eventService;
    private readonly IUserService _userService;

    public EventModel(IEventService eventService, IUserService userService)
    {
        _eventService = eventService;
        _userService = userService;
    }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Event = await _eventService.GetEventByIdAsync(id);
        
        if (Event == null)
        {
            return NotFound();
        }

        // Проверяем, зарегистрирован ли пользователь на это событие
        var userId = User.FindFirst("UserId")?.Value;
        if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var userGuid))
        {
            IsRegistered = Event.Participants.Any(p => p.UserId == userGuid);
        }

        return Page();
    }
}
