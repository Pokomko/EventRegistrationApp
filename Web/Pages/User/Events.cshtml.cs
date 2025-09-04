using Domain.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.Services;

namespace Web.Pages.User;

[Authorize]
public class UserModelIndex: PageModel
{
    public List<Event> Events { get; set; } = new List<Event>();
    public string UserName { get; set; } = string.Empty;

    private readonly IUserRepository _userRepository;
    private readonly IEventService _eventService;

    public UserModelIndex(IUserRepository userRepository, IEventService eventService)
    {
        _userRepository = userRepository;
        _eventService = eventService;
    }

    public async Task OnGetAsync()
    {
        Events = await _eventService.GetAllEventsAsync();

/*        var userId = User.FindFirst("userId")?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            if (Guid.TryParse(userId, out var id))
            {
                var user = await _userRepository.GetByIdAsync(id);

                if (user != null)
                {
                    UserName = user.Username;
                }
            }
        }*/

        ViewData["Title"] = "User - See Events";
    }
}
