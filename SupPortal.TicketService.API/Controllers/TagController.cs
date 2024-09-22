using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TagController : BaseController
{
    public TagController(IMediator mediator) : base(mediator)
    {
    }


    [HttpGet]
    public async Task<IActionResult> Get([FromQuery]GetAllTagsQuery query)
    {
        return (await _mediator.Send(query)).ToActionResult();
    }


    [HttpPost]
    public async Task<IActionResult> Post(CreateTagCommand command)
    {
        return  (await _mediator.Send(command)).ToActionResult();
    }


    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        return  (await _mediator.Send(new DeleteCommentCommand() { CommentId=Id})).ToActionResult();
    }

}
