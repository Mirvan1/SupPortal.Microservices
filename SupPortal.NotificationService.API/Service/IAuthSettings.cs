namespace SupPortal.NotificationService.API.Service;

public interface IAuthSettings
{
     string GetLoggedUsername();
    string GetLoggedUserRole();
    Task<string> GetUser(string username);
}
