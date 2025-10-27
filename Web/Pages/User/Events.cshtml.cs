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
    public string? QueryString { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
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


    private readonly IEventService _eventService;

    public UserModelIndex(IEventService eventService)
    {
        _eventService = eventService;
    }

    public async Task<IActionResult> OnGetAsync(int pageNumber = 1, string? queryString = null, DateTime? startDate = null, DateTime? endDate = null)
    {

        if (User.Identity?.IsAuthenticated != true)
        {
            return RedirectToPage("/NotAuthorized");
        }
        
        CurrentPage = pageNumber;
        QueryString = queryString;
        StartDate = startDate;
        EndDate = endDate;
        
        (Events, TotalCount) = await _eventService.GetPagedEventsAsync(CurrentPage, PageSize, QueryString, StartDate ?? DateTime.Now, EndDate);
        return Page();
    }
}
