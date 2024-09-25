using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Policy;
using System.Net.Http.Headers;
using System.Net.Http;
using Azure.Core;
using Newtonsoft.Json.Linq;

namespace SupPortal.NotificationService.API.Service;

public class AuthSettings(IHttpContextAccessor _httpContextAccessor) : IAuthSettings
{
    public string GetLoggedUsername()
    {
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

    }

    public string GetLoggedUserRole()
    {
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
    }

    public async Task<string> GetUser(string username)
    {
        using var client = new HttpClient();

        var response = await client.GetAsync($"https://localhost:7004/api/user/user-by-username?email={username}");

        if (response.IsSuccessStatusCode)
        {
            dynamic result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            return result.email;
        }
        return null;

    }
}
