using Starter.Application.Features.Common;

namespace Starter.Application.Features.Users.AcceptInvite;
public class AcceptUserInvitationRequest : IRequest<ApiResponse<string>>
{
    public string UserId { get; set; } = default!;
    public string Password { get; set; } = default!;
}
