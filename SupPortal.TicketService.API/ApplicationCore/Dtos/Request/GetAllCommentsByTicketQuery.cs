using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

public class GetAllCommentsByTicketQuery : QueryParameters, IRequest<PaginatedResponseDto<GetCommentDto>>
{
    public int TicketId { get; set; }
}
