using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupPortal.Shared;
public class MQSettings
{
    public const string CreateTicketEvent = "create-ticket-event-queue";
    public const string CreateCommentEvent = "create-comment-event-queue";
    public const string UpdateTicketEvent = "update-ticket-event-queue";
    public const string ForgotPasswordCommand = "forgot-password-command-queue";
    public const string ResetPasswordInfoCommand = "reset-password-command-queue";


}
