using Microsoft.EntityFrameworkCore;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Domain.Entities;
using SupPortal.TicketService.API.Infrastructure.Data;

namespace SupPortal.TicketService.API.Infrastructure.Repository;
public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{
    public TicketRepository(tsContext context) : base(context)
    {
    }
}
