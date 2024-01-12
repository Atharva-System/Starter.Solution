using Starter.Application.Features.Common;

namespace Starter.Application.Models.Users;
public class UpdateUserDto
{
    public string Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    //public string? RoleId { get; set; }
}

public class UpdateUserRequest : IRequest<ApiResponse<string>>
{
    public UpdateUserDto user { get; set; }
    public string Origin { get; set; } = default!;
}
