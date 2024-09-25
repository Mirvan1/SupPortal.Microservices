namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

public class GetTicketDto:BaseResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public int Priority { get; set; }
    public string Username { get; set; }
    public string TagName { get; set; }
}