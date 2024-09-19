using SupPortal.NotificationService.API.Models.Entities;
using SupPortal.NotificationService.API.Repository.Abstract;
using SupPortal.Shared.Events;
using System.CodeDom;
using System.Text.Json;

namespace SupPortal.NotificationService.API.Service;

public class MailService(IRepository<Mail> _mailRepository, IRepository<MailInbox> _mailInboxRepository) : IMailService
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
        var getWaitingMails = await _mailInboxRepository.GetListAsync(x=>x.Where(x=>x.Processed == false));

        if(getWaitingMails is not null)
        {
            foreach(var mail in getWaitingMails)
            {
                mail.Processed=true;

                    var newMail=new Mail()
                {
                    SenderAddress="test@test.com",
                    Username="test",
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
                await _mailInboxRepository.AddAsync(mail);
                await _mailInboxRepository.SaveChangesAsync();



            }
        }
    }
}
