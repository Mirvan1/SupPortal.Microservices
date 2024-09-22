using AutoMapper;
using MassTransit;
using MediatR;
using SupPortal.Shared.Events;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Domain.Entities;
using SupPortal.TicketService.API.Infrastructure.Repository;
using System.Text.Json;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Ticket.Command;

public class CreateTicketCommandHandler(ITicketRepository _ticketRepository, IMapper _mapper, IUnitOfWork _unitOfWork, ITicketOutboxRepository _ticketOutboxRepository, IAuthSettings _authSettings,ILogger<CreateTicketCommandHandler> _logger)
        : IRequestHandler<CreateTicketCommand, BaseResponseDto>

{
    public async Task<BaseResponseDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loggedUserName = _authSettings.GetLoggedUsername();

            if (string.IsNullOrEmpty(loggedUserName)) return BaseResponseDto.ErrorResponse(ConstantErrorMessages.UnAuthorized);

            await _unitOfWork.BeginTransactionAsync();
            _logger.LogInformation("");

            var newTicket = new Domain.Entities.Ticket()
            {
                Name = request.Name,
                Description = request.Description,
                Status = (Status)request.Status,
                Priority = (Priority)request.Priority,
                UserName =   _authSettings.GetLoggedUsername()
            };


            await _ticketRepository.AddAsync(newTicket);

            var mapperEvent = _mapper.Map<CreateTicketEvent>(newTicket);

            mapperEvent.EventIdentifierId = Guid.NewGuid();

            var outboxTicketEvent = new TicketOutbox()
            {
                Id = mapperEvent.EventIdentifierId,
                OccuredOn = DateTime.Now,
                EventType = mapperEvent.GetType().Name,
                EventPayload = JsonSerializer.Serialize(mapperEvent),
                EventStatus = EventStatus.Pending,

            };
            await _ticketOutboxRepository.AddAsync(outboxTicketEvent);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("");

            return BaseResponseDto.SuccessResponse();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(""+e.Message);
            return BaseResponseDto.ErrorResponse(e.Message);

        }
    }
}
