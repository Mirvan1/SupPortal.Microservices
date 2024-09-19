using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Ticket.Query;

public class GetTicketQueryHandler : IRequestHandler<GetTicketQuery, GetTicketDto>
{

    public Task<GetTicketDto> Handle(GetTicketQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
