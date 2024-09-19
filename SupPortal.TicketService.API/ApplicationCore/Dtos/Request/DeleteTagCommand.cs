using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

public class DeleteTagCommand:IRequest<BaseResponseDto>
{
    public int TagId { get; set; }
}
