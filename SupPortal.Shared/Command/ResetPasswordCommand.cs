using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupPortal.Shared.Command;
public class ResetPasswordCommand
{
    public string Email { get; set; }
    public string Username { get; set; }

    public string? IpAddress { get; set; }
}
