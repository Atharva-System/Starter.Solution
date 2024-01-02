using Starter.Application.Features.Common;

namespace Starter.Application.Features.Users.Profile;
public class UpdateProfileRequest : IRequest<ApiResponse<string>>
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? ImageUrl { get; set; }

}
