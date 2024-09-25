using AutoMapper;
using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Ticket.Query;
public class GetAllTicketsQueryHandler(ITicketRepository _ticketRepository,IMapper _mapper)
    : IRequestHandler<GetAllTicketsQuery, PaginatedResponseDto<GetTicketDto>>
{
    public async Task<PaginatedResponseDto<GetTicketDto>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
    {
        //TODO user Auth control

        var getTickets = await _ticketRepository.ToPagedListAsync(new QueryParameters(request.PageNumber,request.PageSize,request.SortBy,request.IsSortDescending),null,cancellationToken );

        if (getTickets is null) return PaginatedResponseDto<GetTicketDto>.Failure(ConstantErrorMessages.NotFound);

        var mappingRes = _mapper.Map<PaginatedResponseDto<GetTicketDto>>(getTickets);

        if (mappingRes is null) return PaginatedResponseDto<GetTicketDto>.Failure(ConstantErrorMessages.NotFound);

        return mappingRes;
    }
}
