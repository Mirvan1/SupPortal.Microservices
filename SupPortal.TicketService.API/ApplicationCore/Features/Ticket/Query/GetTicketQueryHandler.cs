using AutoMapper;
using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Ticket.Query;

public class GetTicketQueryHandler(ITicketRepository _ticketRepository,IMapper _mapper) : IRequestHandler<GetTicketQuery, GetTicketDto>
{

    public async Task<GetTicketDto> Handle(GetTicketQuery request, CancellationToken cancellationToken)
    {

        var getTickets = await _ticketRepository.GetByIdAsync(request.Id);

        if (getTickets is null) return BaseResponseDto.ErrorResponse<GetTicketDto>("");

        var mappingRes = _mapper.Map<GetTicketDto>(getTickets);

        if (mappingRes is null) return BaseResponseDto.ErrorResponse<GetTicketDto>("");

        return mappingRes;
    }
}
