using Starter.Domain.Constant;

namespace Starter.Application.Models.Specification.Filters;
public class PaginationFilter : BaseFilter
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; } = AppConstants.PageSize;

    public string[]? OrderBy { get; set; }
}

public static class PaginationFilterExtensions
{
    public static bool HasOrderBy(this PaginationFilter filter) =>
        filter.OrderBy?.Any() is true;
}
