using MediatR;
using SupPortal.Shared.Events;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Domain.Entities;
using SupPortal.TicketService.API.Infrastructure.Repository;
using System.Text.Json;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Ticket.Command;

public class UpdateTicketStatusCommandHandler(IUnitOfWork _unitOfWork, ITicketRepository _ticketRepository, ITicketOutboxRepository _ticketOutboxRepository, IAuthSettings _authSettings,ILogger<UpdateTicketStatusCommandHandler> _logger) : IRequestHandler<UpdateTicketStatusCommand, BaseResponseDto>
{
    public async Task<BaseResponseDto> Handle(UpdateTicketStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loggedUserRole = _authSettings.GetLoggedUserRole();

            if (loggedUserRole.Equals("User")) return BaseResponseDto.ErrorResponse("");

            await _unitOfWork.BeginTransactionAsync();
            _logger.LogInformation("");

            var getTicket = await _ticketRepository.GetByIdAsync(request.TicketId);

            if (getTicket is null) return BaseResponseDto.ErrorResponse("");

            getTicket.Status = (Status)request.TicketStatus;
            getTicket.UpdateOn = DateTime.Now;

            await _ticketRepository.UpdateAsync(getTicket);

            var identifierId = Guid.NewGuid();

            var updateTicket = new UpdateTicketEvent()
            {
                EventIdentifierId = identifierId,
                UpdatedStatus = (int)getTicket.Status,
                TicketName = getTicket.Name,
                UserName=_authSettings.GetLoggedUsername(),

            };

            var updatetTicketOutbox = new TicketOutbox()
            {
                Id = identifierId,
                OccuredOn = DateTime.Now,
                EventType = updateTicket.GetType().Name,
                EventPayload = JsonSerializer.Serialize(updateTicket),
                EventStatus = EventStatus.Pending
            };
            await _ticketOutboxRepository.AddAsync(updatetTicketOutbox);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("");

            return BaseResponseDto.SuccessResponse();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError("");

            return BaseResponseDto.ErrorResponse(e.Message);
        }

    }
}
