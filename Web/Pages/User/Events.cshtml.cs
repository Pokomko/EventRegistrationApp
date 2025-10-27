using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.User;

[Authorize]
public class UserModelIndex: PageModel
{
    public List<EventDto> Events { get; set; } = new List<EventDto>();
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public string UserName { get; set; } = string.Empty;
    public bool IsAdmin => User.IsInRole("Admin");

    [BindProperty]
    public string EventTitle { get; set; } = string.Empty;

    [BindProperty]
    public string EventDescription { get; set; } = string.Empty;

    [BindProperty]
    public DateTime EventDate { get; set; }

    [BindProperty]
    public string EventLocation { get; set; } = string.Empty;

    [BindProperty]
    public string EventCategory { get; set; } = string.Empty;

    [BindProperty]
    public int EventMaxParticipants { get; set; }

    private readonly IUserService _userService;
    private readonly IEventService _eventService;

    public UserModelIndex(IUserService userService, IEventService eventService)
    {
        _userService = userService;
        _eventService = eventService;
    }

    public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
    {

        if (User.Identity?.IsAuthenticated != true)
        {
            return RedirectToPage("/NotAuthorized");
        }
        
        CurrentPage = pageNumber;
        (Events, TotalCount) = await _eventService.GetPagedEventsAsync(pageNumber, PageSize);
        return Page();
    }
}
