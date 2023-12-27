using Starter.Application.Contracts.Responses;

namespace Starter.Application.Features.Common;
public class PagedApiResponse<T>(int count, int page, int pageSize) : ApiResponse<List<T>>, IPagedDataResponse<T>
{
    public int CurrentPage { get; set; } = page;
    public int PageSize { get; set; } = pageSize;
    public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);
    public int TotalCount { get; set; } = count;
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }

    // Additional properties from ApiResponse class
}
