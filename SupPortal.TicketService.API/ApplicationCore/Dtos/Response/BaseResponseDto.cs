using Newtonsoft.Json;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

public class BaseResponseDto
{
    public bool isSuccess { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? ErrorMessage { get; set; }


    public static T ErrorResponse<T>(string errorMessage) where T : BaseResponseDto, new()
    {
        return new T
        {
            isSuccess = false,
            ErrorMessage = errorMessage
        };
    }

    public static T ErrorResponse<T>(ConstantErrorMessages errorMessage) where T : BaseResponseDto, new()
    {
        return new T
        {
            isSuccess = false,
            ErrorMessage = errorMessage.ToString()
        };
    }

    public static T SuccessResponse<T>() where T : BaseResponseDto, new()
    {
        return new T
        {
            isSuccess = true,
        };
    }


    public static BaseResponseDto ErrorResponse(string errorMessage)
    {
        return new BaseResponseDto
        {
            isSuccess = false,
            ErrorMessage = errorMessage
        };
    }

    public static BaseResponseDto ErrorResponse(ConstantErrorMessages errorMessage)
    {
        return new BaseResponseDto
        {
            isSuccess = false,
            ErrorMessage = errorMessage.ToString()
        };
    }


    public static BaseResponseDto SuccessResponse()
    {
        return new BaseResponseDto
        {
            isSuccess = true,
        };
    }

    public static BaseResponseDto Response(bool result)
    {
        return new BaseResponseDto
        {
            isSuccess = result,
        };
    }
}


public enum ConstantErrorMessages
{
    UnAuthorized=0,
    BadRequest=1,
    NotFound=2,
}