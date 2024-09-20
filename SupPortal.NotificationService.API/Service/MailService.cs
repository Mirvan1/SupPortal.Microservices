using Newtonsoft.Json.Linq;
using SupPortal.NotificationService.API.Models.Entities;
using SupPortal.NotificationService.API.Repository.Abstract;
using SupPortal.Shared.Events;
using System.CodeDom;
using System.Text.Json;

namespace SupPortal.NotificationService.API.Service;

public class MailService(IRepository<Mail> _mailRepository, IRepository<MailInbox> _mailInboxRepository, IAuthSettings _authSettings) : IMailService
{
    public async Task SendMail(Guid identifierId, string Payload, string EventType)
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

            await _mailInboxRepository.SaveChangesAsync();
        }
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

                    switch (mail.EventType)
                    {
                        case nameof(CreateTicketEvent):
                            newMail.Subject = "";
                            newMail.Body = "";
                            break;

                        case nameof(CreateCommentEvent):
                            newMail.Subject = "";
                            newMail.Body = "";
                            break;

                        case nameof(UpdateTicketEvent):
                            newMail.Subject = "";
                            newMail.Body = "";
                            break;
                    }

                    await _mailRepository.AddAsync(newMail);
                    await _mailRepository.SaveChangesAsync();
                    //TODO: imp uow
                    _mailInboxRepository.Update(mail);
                    await _mailInboxRepository.SaveChangesAsync();
                }

                catch (Exception e)
                {
                    mail.EventError = e.Message;
                    mail.Processed = false;
                    _mailInboxRepository.Update(mail);
                    await _mailInboxRepository.SaveChangesAsync();
                    Console.WriteLine(e.Message);
                }


            }
        }

    }
}
