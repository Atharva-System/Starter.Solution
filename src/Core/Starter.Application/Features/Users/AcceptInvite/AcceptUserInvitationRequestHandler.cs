using Starter.Application.Contracts.Identity;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;

namespace Starter.Application.Features.Users.AcceptInvite;
public class AcceptUserInvitationRequestHandler : IRequestHandler<AcceptUserInvitationRequest, ApiResponse<string>>
{
    private readonly IUsersService _userService;
    public AcceptUserInvitationRequestHandler(IUsersService userService)
    {
        _userService = userService;
    }

    public async Task<ApiResponse<string>> Handle(AcceptUserInvitationRequest request, CancellationToken cancellationToken)
    {
        return await _userService.AcceptInvitationAsync(request);
    }
}
