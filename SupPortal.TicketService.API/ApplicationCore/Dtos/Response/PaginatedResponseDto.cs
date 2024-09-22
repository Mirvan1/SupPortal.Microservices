using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.ApplicationCore.Dtos.Response;


public class PaginatedResponseDto<T> : BaseResponseDto
{

    public List<T> Data { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }


    public static PaginatedResponseDto<T> Success(PaginatedList<T> data)
    {
        return new PaginatedResponseDto<T>
        {
            isSuccess = true,
            Data = data.Items,
            PageNumber = data.PageNumber,
            PageSize = data.PageSize,
            TotalPages = data.TotalPages,
            TotalCount = data.TotalCount
        };
    }

    public static PaginatedResponseDto<T> Failure(string errorMessage)
    {
        return new PaginatedResponseDto<T>
        {
            isSuccess = false,
            ErrorMessage = errorMessage
        };
    }

    public static PaginatedResponseDto<T> Failure(ConstantErrorMessages errorMessage)
    {
        return new PaginatedResponseDto<T>
        {
            isSuccess = false,
            ErrorMessage = errorMessage.ToString()
        };
    }

}
