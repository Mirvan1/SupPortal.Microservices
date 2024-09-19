namespace SupPortal.NotificationService.API.Service;

public interface IMailService
{

    Task SendMail(Guid identifierId, string Payload, string EventType);
    Task ProcessMail();
}
