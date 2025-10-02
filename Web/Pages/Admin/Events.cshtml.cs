using Domain.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin;

[Authorize(Policy = "AdminPolicy")]
public class AdminModelIndex : PageModel
{
    public List<Event> Events { get; set; } = new List<Event>();
    public string UserName { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;

    [BindProperty]
    public string EventTitle { get; set; } = string.Empty;

    [BindProperty]
    public string EventDescription { get; set; } = string.Empty;

    [BindProperty]
    public DateTime EventDate { get; set; }

    [BindProperty]
    public string EventLocation { get; set; }

    [BindProperty]
    public string EventCategory { get; set; }

    [BindProperty]
    public int EventMaxParticipants { get; set; }

    private readonly IUserService _userService;
    private readonly IEventService _eventService;

    public AdminModelIndex(IUserService userService, IEventService eventService)
    {
        _userService = userService;
        _eventService = eventService;
    }

    public async Task OnGetAsync()
    {
        Events = await _eventService.GetAllEventsAsync();

/*        
        var userId = User.FindFirst("userId")?.Value;

        if (Guid.TryParse(userId, out var guid))
        {
            var user = await _userRepository.GetByIdAsync(guid);
            if (user != null)
            {
                UserName = user.Username;
                UserRole = user.Roles.FirstOrDefault().Name;     
            }
        }
*/
        ViewData["Title"] = "Admin - Manage Events";
        ViewData["UserRole"] = "Admin";
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var newEvent = new Event();

        if (!ModelState.IsValid)
        {
            Events = await _eventService.GetAllEventsAsync();
            return Page();
        }

        if (EventMaxParticipants <= 0)
        {
            ModelState.AddModelError("EventMaxParticipants", "Max participants must be greater than 0.");
            return Page();
        }

        newEvent.Id = Guid.NewGuid();
        newEvent.Title = EventTitle;
        newEvent.Description = EventDescription;
        newEvent.StartDateTime = EventDate;
        newEvent.Location = EventLocation;
        newEvent.Category = EventCategory;
        newEvent.MaxParticipants = EventMaxParticipants;
        newEvent.ImageUrl = "path";

        await _eventService.CreateEventAsync(newEvent);

        return RedirectToPage();
    }
}
