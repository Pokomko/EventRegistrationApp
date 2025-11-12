using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Web.Pages.User
{
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public ParticipantDto? ParticipantInfo { get; set; } = new ParticipantDto();

        [BindProperty]
        public string FirstName { get; set; } = string.Empty;

        [BindProperty]
        public string LastName { get; set; } = string.Empty;

        [BindProperty]
        public DateTime BirthDate { get; set; }

        private readonly IParticipantService _participantService;

        public ProfileModel(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var userIdGuid))
            {
                ParticipantInfo = await _participantService.GetParticipantByUserIdAsync(userIdGuid);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ParticipantInfo?.FirstName) ||
                    string.IsNullOrWhiteSpace(ParticipantInfo?.LastName))
                {
                    TempData["ErrorMessage"] = "Имя и фамилия обязательны для заполнения.";
                    return Page();
                }

                if (ParticipantInfo.BirthDate > DateTime.Now ||
                    ParticipantInfo.BirthDate < DateTime.Now.AddYears(-120))
                {
                    TempData["ErrorMessage"] = "Неверная дата рождения.";
                    return Page();
                }

                await _participantService.UpdateParticipantAsync(ParticipantInfo);
                TempData["SuccessMessage"] = "Профиль успешно обновлен!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ошибка при обновлении профиля: {ex.Message}";
            }

            // Перезагружаем данные профиля
            var userId = User.FindFirst("userId")?.Value;
            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var userIdGuid))
            {
                ParticipantInfo = await _participantService.GetParticipantByUserIdAsync(userIdGuid);
            }

            return Page();
        }

        public IActionResult OnPostLogout()
        {
            Response.Cookies.Delete("kukuha");
            return RedirectToPage("/Login");
        }
    }
}
