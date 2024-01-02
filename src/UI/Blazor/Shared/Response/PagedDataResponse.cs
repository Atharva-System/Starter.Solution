namespace Starter.Blazor.Shared.Response;

public class PagedDataResponse<T>
{
    public T Data { get; set; }
    public List<string> Messages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}
