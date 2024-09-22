using Microsoft.AspNetCore.Mvc;
using SupPortal.UserService.API.Models.Dto;

namespace SupPortal.UserService.API.Extension;

public static class ControllerExtension
{
    public static IActionResult ToActionResult(this BaseResponse result)
    {
        if (result.IsSuccess)
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

public enum ConstantErrorMessages
{
    UnAuthorized = 0,
    BadRequest = 1,
    NotFound = 2,
}
