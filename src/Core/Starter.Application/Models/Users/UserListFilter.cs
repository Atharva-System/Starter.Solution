using Ardalis.Specification;
using Starter.Application.Models.Specification;
using Starter.Application.Models.Specification.Filters;

namespace Starter.Application.Models.Users;
public class UserListFilter : PaginationFilter
{
}

public class GetSearchUserRequestSpec : EntitiesByPaginationFilterSpec<UserListDto>
{
    public GetSearchUserRequestSpec(UserListFilter request)
    : base(request) =>
        Query.OrderByDescending(c => c.Id, !request.HasOrderBy());
}
