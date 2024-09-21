using MassTransit;
using SupPortal.NotificationService.API.Models.Entities;
using SupPortal.NotificationService.API.Repository.Abstract;
using SupPortal.NotificationService.API.Service;
using SupPortal.Shared.Events;
using System.Text.Json;

namespace SupPortal.NotificationService.API.Consumers;

public class MailNotificationConsumer (IRepository<MailInbox> _mailInboxRepository,IMailService _mailService): IConsumer<CreateTicketEvent>,IConsumer<CreateCommentEvent>, IConsumer<UpdateTicketEvent>
{
    public async Task Consume(ConsumeContext<CreateTicketEvent> context)
    {

       await _mailService.ReceiveMail(context.Message.EventIdentifierId,JsonSerializer.Serialize(context.Message),context.Message.GetType().Name);
    }

    public async Task Consume(ConsumeContext<CreateCommentEvent> context)
    {
        await _mailService.ReceiveMail(context.Message.EventIdentifierId, JsonSerializer.Serialize(context.Message), context.Message.GetType().Name);
    }


    public async Task Consume(ConsumeContext<UpdateTicketEvent> context)
    {
        await _mailService.ReceiveMail(context.Message.EventIdentifierId, JsonSerializer.Serialize(context.Message), context.Message.GetType().Name);
    }



}
