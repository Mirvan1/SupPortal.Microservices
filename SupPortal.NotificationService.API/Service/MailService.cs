using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MimeKit;
using Newtonsoft.Json.Linq;
using SupPortal.NotificationService.API.Models.DTOs;
using SupPortal.NotificationService.API.Models.Entities;
using SupPortal.NotificationService.API.Repository.Abstract;
using SupPortal.Shared.Events;
using System.CodeDom;
using System.Text.Json;
using MailKit.Net.Smtp;
using MailKit;

namespace SupPortal.NotificationService.API.Service;

public class MailService(IRepository<Mail> _mailRepository, IRepository<MailInbox> _mailInboxRepository, IAuthSettings _authSettings, IOptions<MailSettingsDto> _mailSettingsOptions, ILogger<MailService> _logger) : IMailService
{
    private readonly MailSettingsDto _mailSettings = _mailSettingsOptions.Value;


    public async Task ReceiveMail(Guid identifierId, string Payload, string EventType)
    {

        var checkDuplication = await _mailInboxRepository.GetByIdAsync(identifierId);

        if (checkDuplication is null)
        {
            await _mailInboxRepository.AddAsync(new MailInbox()
            {
                Id = identifierId,
                EventPayload = Payload,
                Processed = false,
                EventType = EventType
            });
            _logger.LogInformation("");
            await _mailInboxRepository.SaveChangesAsync();
        }
    }


    public async Task DirectSendEmailConsume(SendEmailDto sendEmail)
    {
        //TODO
        await SendEmail(sendEmail);
    }



    public async Task ProcessMail()
    {

        var getWaitingMails = await _mailInboxRepository.GetListAsync(x => x.Where(x => x.Processed == false));

        if (getWaitingMails is not null)
        {
            foreach (var mail in getWaitingMails)
            {
                try
                {
                    mail.Processed = true;
                    dynamic eventPayload = JObject.Parse(mail.EventPayload);
                    string username = eventPayload.UserName;
                    var newMail = new Mail()
                    {
                        SenderAddress = await _authSettings.GetUser(username),
                        Username = username,
                    };
                    if (!string.IsNullOrEmpty(newMail.SenderAddress) && !string.IsNullOrEmpty(newMail.Username))
                    {
                        string? TicketName = eventPayload.TicketName;
                        switch (mail.EventType)
                        {
                            case nameof(CreateTicketEvent):
                                newMail.Subject = "Your Ticket Created Successfully";
                                newMail.Body = File.ReadAllText(Directory.GetCurrentDirectory() + "/StaticFiles/MailTemplates/create-ticket-template.html").Replace("{Ticket_Name}", TicketName).Replace("{User}", newMail.Username).Replace("{Ticket_Link}", "");

                                _logger.LogInformation("");
                                await SendEmail(new SendEmailDto(newMail.SenderAddress, newMail.Subject, newMail.Body, newMail.Username));

                                break;

                            case nameof(CreateCommentEvent):
                                newMail.Subject = "Your Comment Published Successfully";
                                newMail.Body = File.ReadAllText(Directory.GetCurrentDirectory() + "/StaticFiles/MailTemplates/create-comment-template.html").Replace("{Comment_Link}", "").Replace("{User}", newMail.Username);

                                _logger.LogInformation("");
                                await SendEmail(new SendEmailDto(newMail.SenderAddress, newMail.Subject, newMail.Body, newMail.Username));
                                break;

                            case nameof(UpdateTicketEvent):
                                newMail.Subject = "Your Ticket Updated Successfully";
                                newMail.Body = File.ReadAllText(Directory.GetCurrentDirectory() + "/StaticFiles/MailTemplates/update-ticket-template.html").Replace("{Ticket_Name}", TicketName).Replace("{User}", newMail.Username);

                                _logger.LogInformation("");
                                await SendEmail(new SendEmailDto(newMail.SenderAddress, newMail.Subject, newMail.Body, newMail.Username));
                                break;
                        }

                        await _mailRepository.AddAsync(newMail);
                        await _mailRepository.SaveChangesAsync();
                        //TODO: imp uow
                        _mailInboxRepository.Update(mail);
                        await _mailInboxRepository.SaveChangesAsync();
                    }
                }

                catch (Exception e)
                {
                    mail.EventError = e.Message;
                    mail.Processed = false;
                    _mailInboxRepository.Update(mail);
                    await _mailInboxRepository.SaveChangesAsync();
                    _logger.LogError("" + e.Message);

                    Console.WriteLine(e.Message);
                    throw;
                }


            }
        }

    }


    private async Task SendEmail(SendEmailDto sendEmailDto)
    {
        try
        {
             using (MimeMessage emailMessage = new MimeMessage())
            {
                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(sendEmailDto.Username, sendEmailDto.Email);
                emailMessage.To.Add(emailTo);

                emailMessage.Subject = sendEmailDto.Subject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();

                emailBodyBuilder.HtmlBody = sendEmailDto.Body;

                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                _logger.LogInformation("");
                using (SmtpClient mailClient = new SmtpClient())
                {
                    await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                    await mailClient.SendAsync(emailMessage);
                    await mailClient.DisconnectAsync(true);
                }
            }

        }
        catch
        {
            throw;
        }

    }

}
