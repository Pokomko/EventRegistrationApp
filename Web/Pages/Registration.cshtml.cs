using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class RegistrationModel : PageModel
    {
        private readonly IUserService _userService;

        public RegistrationModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string FirstName { get; set; } = string.Empty;

        [BindProperty]
        public string SecondName { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
            ViewData["Title"] = "Registration";
        }

/*        public async Task<IActionResult> OnPostAsync()
        {
            if (Password != ConfirmPassword) {
                ErrorMessage = "Пароли не совпадают";
                return Page();
            }

            try
            {
                await _service.RegisterAsync(UserName, Password, Email);

                var token = await _service.LoginAsync(Email, Password);

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
                ErrorMessage = "Произошла ошибка при регистрации: " + ex.Message;
                return Page();
            }
        }*/
    }
}
