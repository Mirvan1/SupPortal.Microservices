﻿namespace SupPortal.NotificationService.API.Service;

public interface IMailService
{

    Task ReceiveMail(Guid identifierId, string Payload, string EventType);
    Task ProcessMail();
}
