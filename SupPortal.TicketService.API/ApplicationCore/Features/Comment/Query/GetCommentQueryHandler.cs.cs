using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Comment.Query;

public class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, GetCommentDto>
{
    public Task<GetCommentDto> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}