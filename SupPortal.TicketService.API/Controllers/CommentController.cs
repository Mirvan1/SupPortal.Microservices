using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.Infrastructure.Extension;

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
        return  (await _mediator.Send(query)).ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateCommentCommand command)
    {
        return  (await _mediator.Send(command)).ToActionResult();
    }




}
