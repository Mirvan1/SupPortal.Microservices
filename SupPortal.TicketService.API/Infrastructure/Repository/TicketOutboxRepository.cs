using Microsoft.EntityFrameworkCore;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Domain.Entities;
using SupPortal.TicketService.API.Infrastructure.Data;

namespace SupPortal.TicketService.API.Infrastructure.Repository;

public class TicketOutboxRepository : GenericRepository<TicketOutbox>, ITicketOutboxRepository
{
    public TicketOutboxRepository(tsContext context) : base(context)
    {
    }

    public async Task<List<TicketOutbox>> GetPendingEvents()
    {
        return await _dbSet.AsNoTracking().Where(x => x.EventStatus == EventStatus.Pending).ToListAsync();
    }
}
