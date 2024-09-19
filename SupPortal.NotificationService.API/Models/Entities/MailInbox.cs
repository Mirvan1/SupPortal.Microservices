namespace SupPortal.NotificationService.API.Models.Entities;

public class MailInbox
{
    public Guid Id { get; set; }
    public string EventPayload { get; set; }
    public string EventType { get; set; }
    public bool Processed { get; set; }
}
