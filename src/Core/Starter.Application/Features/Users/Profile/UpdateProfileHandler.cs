using Starter.Application.Contracts.Identity;
using Starter.Application.Features.Common;

namespace Starter.Application.Features.Users.Profile;
public class UpdateProfileHandler : IRequestHandler<UpdateProfileRequest, ApiResponse<string>>
{
    private readonly IUsersService _usersService;

    public UpdateProfileHandler(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async Task<ApiResponse<string>> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        
        return await _usersService.UpdateProfileAsync(request);
    }
}
