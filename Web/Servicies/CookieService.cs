using Application.Interfaces;

namespace Web.Servicies;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CookieService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetAuthCookie(string key) {
        return _httpContextAccessor.HttpContext?.Request.Cookies[key]; ;
    }
    public void SetAuthCookie(string key, string value, int? expireHours = null)
    {
        var options = new CookieOptions();

        if (expireHours.HasValue) {
            options.Expires = DateTime.UtcNow.AddHours(expireHours.Value);
        }

        _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value);

    }

    public void DeleteAuthCookie(string key)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
    }
}
