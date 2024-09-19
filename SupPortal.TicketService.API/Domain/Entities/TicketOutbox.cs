using MassTransit;

namespace SupPortal.TicketService.API.Domain.Entities;

public class TicketOutbox
{

    public Guid Id { get; set; }
    public string EventType { get; set; }
    public string EventPayload { get; set; }
    public DateTime OccuredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public EventStatus EventStatus { get; set; }
}

public enum EventStatus
{
    Pending=0,
    Sending=1,
    Completed=2,
    Failed=3
}
