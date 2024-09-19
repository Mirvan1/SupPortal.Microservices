using SupPortal.TicketService.API.Domain.Entities;

namespace SupPortal.TicketService.API.ApplicationCore.Interface;

public interface ITicketOutboxRepository : IGenericRepository<TicketOutbox>
{

    Task<List<TicketOutbox>> GetPendingEvents();
}
