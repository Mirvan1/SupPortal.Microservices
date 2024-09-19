using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Ticket.Command;

public class DeleteTicketCommandHandler(IAuthSettings _authSettings,ITicketRepository _ticketRepository) : IRequestHandler<DeleteTicketCommand, BaseResponseDto>
{
    public async Task<BaseResponseDto> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var loggedUserRole = _authSettings.GetLoggedUserRole();
        var loggedUserName = _authSettings.GetLoggedUsername();

        var getTicket = await _ticketRepository.GetByIdAsync(request.TicketId);

        if (getTicket is null) return BaseResponseDto.ErrorResponse("");

        if(!getTicket.UserName.Equals(loggedUserName) || loggedUserRole.Equals("Supporter")) return BaseResponseDto.ErrorResponse("");

        await _ticketRepository.DeleteAsync(getTicket);
        await _ticketRepository.SaveChangesAsync();

        return BaseResponseDto.SuccessResponse();
    }
}
