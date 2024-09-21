using MassTransit;
using SupPortal.Shared;
using SupPortal.Shared.Events;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using System.Text.Json;
using System.Windows.Input;

namespace SupPortal.TicketService.API.ApplicationCore.Services;

public  class PublishJobService(ITicketOutboxRepository _ticketOutboxRepository,IPublishEndpoint _publishEndpoint) : IPublishJobService
{
    public  async Task TicketOutboxPublishJob()
    {
        var getPendingEvents = await _ticketOutboxRepository.GetPendingEvents();

        if(getPendingEvents is not null && getPendingEvents?.Count>0)
        {
            foreach( var pendingEvent in getPendingEvents)
            {
              pendingEvent.EventStatus = Domain.Entities.EventStatus.Sending;

                switch(pendingEvent.EventType)
                {
                    case nameof(CreateTicketEvent):
                        CreateTicketEvent createTicketEvent = JsonSerializer.Deserialize<CreateTicketEvent>(pendingEvent.EventPayload);
                        await _publishEndpoint.Publish(createTicketEvent,
                            x =>
                            {
                                x.SetRoutingKey(MQSettings.CreateTicketEvent);
                            });
                        break;
                    case nameof(CreateCommentEvent):
                        CreateCommentEvent  createCommentEvent = JsonSerializer.Deserialize<CreateCommentEvent>(pendingEvent.EventPayload);
                        await _publishEndpoint.Publish(createCommentEvent,
                            x =>
                            {
                                x.SetRoutingKey(MQSettings.CreateCommentEvent);
                            });
                        break;
                    case nameof(UpdateTicketEvent):
                        UpdateTicketEvent updateTicketEvent = JsonSerializer.Deserialize<UpdateTicketEvent>(pendingEvent.EventPayload);
                        await _publishEndpoint.Publish(updateTicketEvent,
                            x =>
                            {
                                x.SetRoutingKey(MQSettings.UpdateTicketEvent);
                            });
                        break;
                }

                pendingEvent.ProcessedOn = DateTime.Now;
                await _ticketOutboxRepository.UpdateAsync(pendingEvent);
                await _ticketOutboxRepository.SaveChangesAsync();
            }
        }

    }
}
