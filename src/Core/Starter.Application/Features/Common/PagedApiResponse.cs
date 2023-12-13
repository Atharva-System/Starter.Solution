using Starter.Application.Contracts.Responses;

namespace Starter.Application.Features.Common;
public class PagedApiResponse<T> : ApiResponse<List<T>>, IPagedDataResponse<T>
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }

    // Additional properties from ApiResponse class

    public List<string> Messages { get; set; } = new List<string>();
}
