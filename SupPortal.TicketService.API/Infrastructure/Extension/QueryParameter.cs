namespace SupPortal.TicketService.API.Infrastructure.Extension;
public class QueryParameters
{
    public int PageNumber { get; set; } = 0;
    public int PageSize { get; set; } = int.MaxValue;
    public string? SortBy { get; set; } = string.Empty;
    public bool? IsSortDescending { get; set; }
    public string? SearchBy { get; set; } = string.Empty;
    public string? SearchText { get; set; } = string.Empty;


    public QueryParameters(int pageNumber,int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public QueryParameters(int pageNumber, int pageSize, string? sortBy, bool? isSortDescending)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SortBy = sortBy;
        IsSortDescending = isSortDescending;
    }

    public QueryParameters()
    {
        
    }
}