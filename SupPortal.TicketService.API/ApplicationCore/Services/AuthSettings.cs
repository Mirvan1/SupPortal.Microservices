using SupPortal.TicketService.API.ApplicationCore.Interface;
using System.Security.Claims;

namespace SupPortal.TicketService.API.ApplicationCore.Services;

public class AuthSettings(IHttpContextAccessor _httpContextAccessor) :IAuthSettings
{
    public string GetLoggedUsername()
    { 
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        
    }

    public string GetLoggedUserRole()
    {
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

    }
}
