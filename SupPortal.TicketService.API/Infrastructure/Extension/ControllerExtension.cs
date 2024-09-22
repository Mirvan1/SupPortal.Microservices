using Microsoft.AspNetCore.Mvc;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using System.Diagnostics.Eventing.Reader;

namespace SupPortal.TicketService.API.Infrastructure.Extension;

public static class ControllerExtension
{
    public static IActionResult ToActionResult(this BaseResponseDto result)
    {
        if (result.isSuccess)
        {
            return new OkObjectResult(result);  
        }
        else
        {
            switch (result.ErrorMessage)
            {
                case nameof(ConstantErrorMessages.UnAuthorized):
                    return new UnauthorizedObjectResult(result);    
                case nameof(ConstantErrorMessages.BadRequest):
                    return new BadRequestObjectResult(result);
                case nameof(ConstantErrorMessages.NotFound):
                    return new NotFoundObjectResult(result);
                default:
                    return new UnprocessableEntityObjectResult(result);
            }
        }
    }
}


