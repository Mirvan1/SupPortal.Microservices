namespace SupPortal.TicketService.API.Domain.Entities;

    public class Comment:BaseEntity
    {
     public int TicketId { get; set; }
    public string UserName { get; set; }
    public string Content { get; set; }
 
    public Ticket Ticket { get; set; }
}
