namespace SupPortal.TicketService.API.ApplicationCore.Interface;

public interface IAuthSettings
{
     string GetLoggedUsername();
    string GetLoggedUserRole();
}
