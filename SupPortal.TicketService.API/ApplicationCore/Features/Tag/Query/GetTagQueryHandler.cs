using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Tag.Query;

public class GetTagQueryHandler : IRequestHandler<GetTagQuery, GetTagDto>
{
    public Task<GetTagDto> Handle(GetTagQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
