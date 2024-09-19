using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

namespace SupPortal.TicketService.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TicketController : BaseController
{
    public TicketController(IMediator mediator) : base(mediator)
    {
    }


    [HttpGet]
    public async Task<IActionResult> Get(GetTicketQuery query)
    {
        return Ok(await _mediator.Send(query));
    }


    [HttpPost]
    public async Task<IActionResult> Post(CreateTicketCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        return Ok(await _mediator.Send(new DeleteTicketCommand() { TicketId = Id }));
    }


    [HttpPatch]
    public async Task<IActionResult> Patch(UpdateTicketStatusCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

}
