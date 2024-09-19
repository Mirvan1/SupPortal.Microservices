using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.Domain.Entities;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

public class CreateTicketCommand: IRequest<BaseResponseDto>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public int Priority { get; set; }
    public int TagId { get; set; }

}
