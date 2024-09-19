namespace SupPortal.TicketService.API.Domain.Entities;

    public class Tag:BaseEntity
    {
        public string Name { get; set; }

    public string UserName { get; set; }
    public List<Ticket>  Tickets { get; set; }
}
