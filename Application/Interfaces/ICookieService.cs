using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface ICookieService
{
    string? GetAuthCookie(string key);
    void SetAuthCookie(string key, string value, int? expireHours = null);
    void DeleteAuthCookie(string key);
}
