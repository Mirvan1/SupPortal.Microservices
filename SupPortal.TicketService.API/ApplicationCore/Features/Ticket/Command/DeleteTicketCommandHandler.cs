using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Ticket.Command;

public class DeleteTicketCommandHandler(IAuthSettings _authSettings,ITicketRepository _ticketRepository,ILogger<DeleteTicketCommandHandler> _logger) : IRequestHandler<DeleteTicketCommand, BaseResponseDto>
{
    public async Task<BaseResponseDto> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var loggedUserRole = _authSettings.GetLoggedUserRole();
        var loggedUserName = _authSettings.GetLoggedUsername();

        var getTicket = await _ticketRepository.GetByIdAsync(request.TicketId);

        if (getTicket is null) return BaseResponseDto.ErrorResponse(ConstantErrorMessages.NotFound);

        if(!getTicket.UserName.Equals(loggedUserName) || loggedUserRole.Equals("Supporter")) return BaseResponseDto.ErrorResponse(ConstantErrorMessages.UnAuthorized);

        await _ticketRepository.DeleteAsync(getTicket);
        int res = await _ticketRepository.SaveChangesAsync();

        _logger.LogInformation("");

        return BaseResponseDto.Response(res>0);
    }
}
