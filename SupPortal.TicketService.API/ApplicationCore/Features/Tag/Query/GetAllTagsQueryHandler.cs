using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Tag.Query;
public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, PaginatedResponseDto<GetTagDto>>
{
    public Task<PaginatedResponseDto<GetTagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

