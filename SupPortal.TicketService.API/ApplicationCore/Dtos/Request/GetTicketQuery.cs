using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

public class GetTicketQuery : IRequest<GetTicketDto>
{
    public int Id { get; set; }

}

