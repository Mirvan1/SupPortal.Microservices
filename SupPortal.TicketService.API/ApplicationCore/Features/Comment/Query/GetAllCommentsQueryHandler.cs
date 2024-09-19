using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Comment.Query;

public class GetAllCommentsQueryHandler
    : IRequestHandler<GetAllCommentsByTicketQuery, PaginatedResponseDto<GetCommentDto>>
{
    public Task<PaginatedResponseDto<GetCommentDto>> Handle(GetAllCommentsByTicketQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
