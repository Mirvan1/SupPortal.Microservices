namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

public class GetCommentDto
{
    public int TicketId { get; set; }
    public string UserName { get; set; }
    public string Content { get; set; }
}
