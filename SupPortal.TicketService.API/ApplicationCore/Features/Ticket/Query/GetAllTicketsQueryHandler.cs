using AutoMapper;
using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Ticket.Query;
public class GetAllTicketsQueryHandler(ITicketRepository _ticketRepository,IMapper _mapper)
    : IRequestHandler<GetAllTicketsQuery, PaginatedResponseDto<GetTicketDto>>
{
    public async Task<PaginatedResponseDto<GetTicketDto>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
    {
        //user Auth control

        var getTickets = await _ticketRepository.ToPagedListAsync();

        if (getTickets is null) return PaginatedResponseDto<GetTicketDto>.Failure("");

        var mappingRes = _mapper.Map<PaginatedResponseDto<GetTicketDto>>(getTickets);

        if (mappingRes is null) return PaginatedResponseDto<GetTicketDto>.Failure("");

        return mappingRes;
    }
}
