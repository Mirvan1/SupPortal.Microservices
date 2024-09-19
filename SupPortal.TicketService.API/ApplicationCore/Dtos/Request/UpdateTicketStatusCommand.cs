using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

public class UpdateTicketStatusCommand : IRequest<BaseResponseDto>
{
    public int TicketId { get; set; }
    public int TicketStatus { get; set; }
}
