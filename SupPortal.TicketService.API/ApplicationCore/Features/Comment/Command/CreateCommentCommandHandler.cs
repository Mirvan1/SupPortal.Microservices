using MassTransit;
using MediatR;
using SupPortal.Shared.Events;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Domain.Entities;
using System.Text.Json;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Comment.Command;

public class CreateCommentCommandHandler(ITicketRepository _ticketRepository, ICommentRepository _commentRepository, IUnitOfWork _unitOfWork, ITicketOutboxRepository _ticketOutboxRepository, IAuthSettings _authSettings, ILogger<CreateCommentCommandHandler> _logger)
        : IRequestHandler<CreateCommentCommand, BaseResponseDto>

{
    public async Task<BaseResponseDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var getTicket = await _ticketRepository.GetByIdAsync(request.TicketId);

            if (getTicket is null) return BaseResponseDto.ErrorResponse(ConstantErrorMessages.BadRequest);

            await _unitOfWork.BeginTransactionAsync();
            _logger.LogInformation("");

            var newComment = new Domain.Entities.Comment()
            {
                Content = request.CommentContent,
                TicketId = request.TicketId,
                UserName = _authSettings.GetLoggedUsername()
            };

            await _commentRepository.AddAsync(newComment);


            var identifierId = Guid.NewGuid();
            var commentEvent = new CreateCommentEvent()
            {
                TicketOwnerUsername = getTicket.UserName,
                CommentOwnerUsername = _authSettings.GetLoggedUsername(),
                CommentContent = request.CommentContent,
                EventIdentifierId = identifierId
            };


            var outboxCommentEvent = new TicketOutbox()
            {
                Id = identifierId,
                OccuredOn = DateTime.Now,
                EventType = commentEvent.GetType().Name,
                EventPayload = JsonSerializer.Serialize(commentEvent),
                EventStatus = EventStatus.Pending
            };

            await _ticketOutboxRepository.AddAsync(outboxCommentEvent);
            // await _ticketOutboxRepository.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            _logger.LogInformation("");


            return BaseResponseDto.SuccessResponse();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError("" + e.Message);

            return BaseResponseDto.ErrorResponse(e.Message);

        }
    }
}
