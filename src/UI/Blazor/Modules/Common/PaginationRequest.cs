namespace Starter.Blazor.Modules.Common;

public class PaginationRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string[] OrderBy { get; set; }
}
