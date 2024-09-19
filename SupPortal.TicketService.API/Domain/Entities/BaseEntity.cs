namespace SupPortal.TicketService.API.Domain.Entities;
    public class BaseEntity
    {
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdateOn { get; set; }
}

