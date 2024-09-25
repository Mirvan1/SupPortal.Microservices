using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Infrastructure.Repository;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Comment.Command;

public class DeleteCommentCommandHandler(IAuthSettings _authSettings, ICommentRepository _commentRepository) : IRequestHandler<DeleteCommentCommand, BaseResponseDto>
{
    public async Task<BaseResponseDto> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var loggedUserRole = _authSettings.GetLoggedUserRole();
        var loggedUserName = _authSettings.GetLoggedUsername();

        var getComment = await _commentRepository.GetByIdAsync(request.CommentId,cancellationToken);

        if (getComment is null) return BaseResponseDto.ErrorResponse(ConstantErrorMessages.BadRequest);

        if (!getComment.UserName.Equals(loggedUserName) || loggedUserRole.Equals("Supporter")) return BaseResponseDto.ErrorResponse(ConstantErrorMessages.UnAuthorized);

        await _commentRepository.DeleteAsync(getComment);
        await _commentRepository.SaveChangesAsync(cancellationToken);

        return BaseResponseDto.SuccessResponse();
    }
}
