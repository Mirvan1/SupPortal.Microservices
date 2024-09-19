using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupPortal.Shared.Events;
public class CreateTicketEvent
{
    public string TicketName { get; set; }
    public string TicketDescription { get; set; }

    public int Status { get; set; }
    public int Priority { get; set; }
    public string UserName { get; set; }

    public Guid EventIdentifierId { get; set; }
}
