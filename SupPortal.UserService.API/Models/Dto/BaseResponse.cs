using Newtonsoft.Json;
using SupPortal.UserService.API.Extension;

namespace SupPortal.UserService.API.Models.Dto;

public   class BaseResponse
{
    public bool IsSuccess { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? ErrorMessage { get; set; }

    public static BaseResponse Response(bool result)
    {
        return new BaseResponse
        {
            IsSuccess = result,
        };
    }

    public static T Response<T>(bool result) where T : BaseResponse, new()
    {
        return new T
        {
            IsSuccess = result,
        };


    }

    public static T ErrorResponse<T>(ConstantErrorMessages errorMessage) where T : BaseResponse, new()
    {
        return new T
        {
            IsSuccess = false,
            ErrorMessage = errorMessage.ToString()
        };


    }

    public static BaseResponse ErrorResponse(ConstantErrorMessages errorMessage)
    {
        return new BaseResponse
        {
            IsSuccess = false,
            ErrorMessage = errorMessage.ToString()
        };
    }
}
