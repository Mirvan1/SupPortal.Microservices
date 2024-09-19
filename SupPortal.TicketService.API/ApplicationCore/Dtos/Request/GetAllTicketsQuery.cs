using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

public class GetAllTicketsQuery : QueryParameters, IRequest<PaginatedResponseDto<GetTicketDto>>
{

}
