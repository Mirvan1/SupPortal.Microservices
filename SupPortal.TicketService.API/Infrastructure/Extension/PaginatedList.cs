using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;

namespace SupPortal.TicketService.API.Infrastructure.Extension;


public class PaginatedList<T>:BaseResponseDto
{
    public List<T> Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool IsSuccess { get; set; } = false;
    public PaginatedList()
    {
    }
    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = count;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = items;
        if (Items.Count >= 0 && count >= 0)
        {
            IsSuccess = true;
        }
    }



    public static PaginatedList<T> Failure(string errorMessage)
    {
        return new PaginatedList<T>
        {
            isSuccess = false,
            ErrorMessage = errorMessage
        };
    }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
