using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

public class DeleteCommentCommand:IRequest<BaseResponseDto>
{
    public int CommentId { get; set; }
}
