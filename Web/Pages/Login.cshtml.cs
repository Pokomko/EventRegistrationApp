using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

public class LoginModel : PageModel
{
    private readonly UserService _service;

    public LoginModel(UserService service)
    {
        _service = service;
    }

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;
    public void OnGet() {
        ViewData["Title"] = "Login";
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var token = await _service.Login(Email, Password);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            Response.Cookies.Append("kukuha", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return role switch
            {
                "Admin" => Redirect("/Admin/Events"),
                "User" => Redirect("/User/Events"),
                _ => Redirect("/")
            };
        }
        catch (Exception ex)
        {
            ErrorMessage = "Неверный email или пароль.";
            return Page();
        }
    }
}