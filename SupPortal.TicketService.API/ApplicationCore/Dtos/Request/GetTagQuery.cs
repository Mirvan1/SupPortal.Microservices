using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

public class GetTagQuery : IRequest<GetTagDto>
{
    public int Id { get; set; }

}
