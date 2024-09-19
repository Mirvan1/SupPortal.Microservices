namespace SupPortal.TicketService.API.Infrastructure.Extension;
public class QueryParameters
{
    public int PageNumber { get; set; } = 0;
    public int PageSize { get; set; } = int.MaxValue;
    public string? SortBy { get; set; } = string.Empty;
    public bool? IsSortDescending { get; set; }
    public string? SearchBy { get; set; } = string.Empty;
    public string? SearchText { get; set; } = string.Empty;


}