namespace SupPortal.NotificationService.API.Models.Entities;

public class Mail
{
    public int Id { get; set; }
    public string SenderAddress { get; set; }
    public string Username { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

    public DateTime SendDate { get; set; } = DateTime.UtcNow;

}
