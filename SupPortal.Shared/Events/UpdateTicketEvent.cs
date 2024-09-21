using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupPortal.Shared.Events;
public class UpdateTicketEvent
{
    public string TicketName { get; set; }
    public int UpdatedStatus { get; set; }

    public Guid EventIdentifierId { get; set; }

    public string UserName { get; set; }

}
