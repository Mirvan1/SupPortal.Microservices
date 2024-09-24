using SupPortal.NotificationService.API.Models.DTOs;

namespace SupPortal.NotificationService.API.Service;

public interface IMailService
{

    Task ReceiveMail(Guid identifierId, string Payload, string EventType);
    Task DirectSendEmailConsume(SendEmailDto sendEmail);
    Task ProcessMail();
}
