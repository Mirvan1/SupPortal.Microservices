namespace SupPortal.NotificationService.API.Models.DTOs;

public class SendEmailDto
{
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Username { get; set; }

    public SendEmailDto(string email, string subject, string body,string username)
    {
        Email = email;
        Subject = subject;
        Body = body;
        Username = username;

    }
}
