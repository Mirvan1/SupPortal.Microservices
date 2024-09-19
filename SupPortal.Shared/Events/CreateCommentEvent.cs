using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupPortal.Shared.Events;
public class CreateCommentEvent
{
    public string TicketOwnerUsername{ get; set; }
    public string CommentOwnerUsername { get; set; }
    public string CommentContent { get; set; }
    public Guid EventIdentifierId { get; set; }

}
    