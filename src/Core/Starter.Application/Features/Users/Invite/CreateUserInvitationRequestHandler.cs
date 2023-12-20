using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starter.Application.Contracts.Identity;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;

namespace Starter.Application.Features.Users.Invite;
public class CreateUserInvitationRequestHandler : IRequestHandler<CreateUserInvitationRequest, ApiResponse<string>>
{
    private readonly IUsersService _userService;

    public CreateUserInvitationRequestHandler(IUsersService userService)
    {
        _userService = userService;
    }

    public async Task<ApiResponse<string>> Handle(CreateUserInvitationRequest request, CancellationToken cancellationToken)
    {
        return await _userService.CreateInvitationAsync(request.request, request.Origion);
    }
}
