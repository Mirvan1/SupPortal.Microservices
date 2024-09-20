using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

namespace SupPortal.TicketService.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CommentController : BaseController
{
    public CommentController(IMediator mediator) : base(mediator)
    {
    }


    [HttpGet]
    public async Task<IActionResult> Get([FromQuery]GetAllCommentsByTicketQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateCommentCommand command)
    {
        return Ok(await _mediator.Send(command));
    }




}
