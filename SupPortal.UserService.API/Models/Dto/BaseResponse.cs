using Newtonsoft.Json;

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
}
