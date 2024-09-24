using MassTransit;
using SupPortal.NotificationService.API.Models.DTOs;
using SupPortal.NotificationService.API.Models.Entities;
using SupPortal.NotificationService.API.Repository.Abstract;
using SupPortal.NotificationService.API.Service;
using SupPortal.Shared.Command;
using SupPortal.Shared.Events;
using System.Text.Json;

namespace SupPortal.NotificationService.API.Consumers;

public class MailNotificationConsumer(IRepository<MailInbox> _mailInboxRepository, IMailService _mailService)
    : IConsumer<CreateTicketEvent>, IConsumer<CreateCommentEvent>,
    IConsumer<UpdateTicketEvent>, IConsumer<ForgotPasswordCommand>, IConsumer<ResetPasswordCommand>
{
    public async Task Consume(ConsumeContext<CreateTicketEvent> context)
    {

        await _mailService.ReceiveMail(context.Message.EventIdentifierId, JsonSerializer.Serialize(context.Message), context.Message.GetType().Name);
    }

    public async Task Consume(ConsumeContext<CreateCommentEvent> context)
    {
        await _mailService.ReceiveMail(context.Message.EventIdentifierId, JsonSerializer.Serialize(context.Message), context.Message.GetType().Name);
    }


    public async Task Consume(ConsumeContext<UpdateTicketEvent> context)
    {
        await _mailService.ReceiveMail(context.Message.EventIdentifierId, JsonSerializer.Serialize(context.Message), context.Message.GetType().Name);
    }

    public async Task Consume(ConsumeContext<ForgotPasswordCommand> context)
    {
        //without inbox pattern
        var sendEmail = new SendEmailDto
        (context.Message.Email, "Reset password",
        File.ReadAllText(Directory.GetCurrentDirectory() + "/StaticFiles/MailTemplates/forgot-password-template.html").Replace("{Reset_Url}", context.Message.ResetPasswordUrl),
        context.Message.Username
        );
        await _mailService.DirectSendEmailConsume(sendEmail);
    }

    public async Task Consume(ConsumeContext<ResetPasswordCommand> context)
    {
        //without inbox pattern
        var sendEmail = new SendEmailDto
               (context.Message.Email, "Your password successfully changed",
               File.ReadAllText(Directory.GetCurrentDirectory() + "/StaticFiles/MailTemplates/reset-password-info.html").Replace("{Reset_Date_Time}", DateTime.Now.ToLongDateString()).Replace("{IP_Address}", context.Message.IpAddress),
               context.Message.Username
               );
        await _mailService.DirectSendEmailConsume(sendEmail);

    }
}
