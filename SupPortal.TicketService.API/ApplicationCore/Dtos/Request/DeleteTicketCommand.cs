using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

public class DeleteTicketCommand : IRequest<BaseResponseDto>
{
    public int TicketId { get; set; }
}
