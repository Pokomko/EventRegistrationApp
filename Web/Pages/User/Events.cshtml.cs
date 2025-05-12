using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Web.Pages.User;

[Authorize]
public class UserModelIndex: PageModel
{
    public List<Event> Events { get; set; } = new List<Event>();
    public string UserName { get; set; } = string.Empty;

    private readonly IUserRepository _repository;
    private readonly AppDbContext _context;

    public UserModelIndex(AppDbContext context, IUserRepository repository)
    {
        _repository = repository;
        _context = context;
    }

    public async Task OnGetAsync()
    {
        Events = await _context.Events.ToListAsync();

        var userId = User.FindFirst("userId")?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            if (Guid.TryParse(userId, out var id))
            {
                var user = await _repository.GetByIdAsync(id);

                if (user != null)
                {
                    UserName = user.Username;
                }
            }
        }

        ViewData["Title"] = "User - See Events";
    }
}
