namespace SupPortal.TicketService.API.Domain.Entities;

    public class Ticket:BaseEntity
    {
    public string Name{ get; set; }
    public string Description { get; set; }
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public string UserName { get; set; }
    public int TagId { get; set; }

    public List<Comment> Comments { get; set; } 
    public List<Tag> TicketTags { get; set; } 
}


public enum Status
{
    Open=0,
    Progress=1,
    Close=2
}

public enum Priority
{
    Low=0,
    Medium=1,
    High=2
}