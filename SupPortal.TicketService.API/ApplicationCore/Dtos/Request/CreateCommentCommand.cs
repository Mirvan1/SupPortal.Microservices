using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

public class CreateCommentCommand:IRequest<BaseResponseDto>
{
    public int TicketId { get; set; }
    public string CommentContent { get; set; }


}
