using Hangfire;
using SupPortal.TicketService.API.ApplicationCore.Interface;

namespace SupPortal.TicketService.API.ApplicationCore.Jobs;

public class OutboxPublishJob(IRecurringJobManager _recurringJobManager)
{
    public  void TicketOutboxJob()
    {
        _recurringJobManager.RemoveIfExists(nameof(OutboxPublishJob));
        _recurringJobManager.AddOrUpdate<IPublishJobService>("TicketOutboxJob", service => service.TicketOutboxPublishJob(), "*/5 * * * *");

    }
}
