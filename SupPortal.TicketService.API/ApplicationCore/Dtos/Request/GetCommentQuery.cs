using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

    public class GetCommentQuery:IRequest<GetCommentDto>
    {
        public int Id { get; set; }

    }
