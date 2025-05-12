using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Web.Pages.Admin;

[Authorize(Policy = "AdminPolicy")]
public class AdminModelIndex : PageModel
{
    public List<Event> Events { get; set; } = new();
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

    private readonly IUserRepository _repository;
    private readonly AppDbContext _context;

    public AdminModelIndex(AppDbContext context, IUserRepository repository)
    {
        _context = context;
        _repository = repository;
    }

    public async Task OnGetAsync()
    {
        Events = await _context.Events.ToListAsync();

        var userId = User.FindFirst("userId")?.Value;

        if (Guid.TryParse(userId, out var guid))
        {
            var user = await _repository.GetByIdAsync(guid);
            if (user != null)
            {
                UserName = user.Username;
                UserRole = user.Roles.FirstOrDefault().Name;     
            }
        }

        ViewData["Title"] = "Admin - Manage Events";
        ViewData["UserRole"] = UserRole;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var newEvent = new Event();

        if (!ModelState.IsValid)
        {
            Events = await _context.Events.ToListAsync();
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

        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}
