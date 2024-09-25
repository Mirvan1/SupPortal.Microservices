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


    [HttpGet("{Name}")]
    public async Task<IActionResult> Get(string Name)
    {
        return (await _mediator.Send(new GetTagQuery() { TagName = Name })).ToActionResult();
    }


    [HttpPost]
    public async Task<IActionResult> Post(CreateTagCommand command)
    {
        return  (await _mediator.Send(command)).ToActionResult();
    }


    [HttpDelete("{Name}")]
    public async Task<IActionResult> Delete(string Name)
    {
        return  (await _mediator.Send(new DeleteTagCommand() { TagName = Name})).ToActionResult();
    }

}
