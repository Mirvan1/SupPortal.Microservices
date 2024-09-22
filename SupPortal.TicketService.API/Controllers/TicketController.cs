using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.Infrastructure.Extension;

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
    public async Task<IActionResult> Get([FromQuery] GetAllTicketsQuery query)
    {
        return  (await _mediator.Send(query)).ToActionResult();
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(int Id)
    {
        return(await _mediator.Send(new GetTicketQuery() { Id = Id })).ToActionResult();
    }


    [HttpPost]
    public async Task<IActionResult> Post(CreateTicketCommand command)
    {
        return  (await _mediator.Send(command)).ToActionResult();
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        return  (await _mediator.Send(new DeleteTicketCommand() { TicketId = Id })).ToActionResult();
    }


    [HttpPatch]
    public async Task<IActionResult> Patch(UpdateTicketStatusCommand command)
    {
        return  (await _mediator.Send(command)).ToActionResult();
    }

}
