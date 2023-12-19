using Starter.Application.Features.Common;
using Starter.Application.Models.Users;

namespace Starter.Application.Contracts.Identity;
public interface IUsersService : ITransientService
{
    Task<ApiResponse<UserDetailsDto>> GetUserDetailsAsync (string userId, CancellationToken cancellationToken);
    Task<ApiResponse<string>> UpdateAsync (UpdateUserDto request);
}
