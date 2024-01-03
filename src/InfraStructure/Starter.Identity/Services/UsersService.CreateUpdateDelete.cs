using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Mailing;
using Starter.Application.Contracts.Mailing.Models;
using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.Features.Users.AcceptInvite;
using Starter.Application.Features.Users.Invite;
using Starter.Application.Models.Users;
using Starter.Identity.Models;

namespace Starter.Identity.Services;
public partial class UsersService
{
    public async Task<ApiResponse<string>> CreateInvitationAsync(CreateUserInvitation request, string origin)
    {
        var applicationUser = await ExistsUserWithEmailAsync(request.Email);
        if (applicationUser == true)
        {
            throw new CustomException("User is already exist.");
        }
        else
        {
            var user = new ApplicationUser()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                IsActive = false,
                InvitedBy = new Guid(_currentUserService.UserId),
                InvitedDate = DateTime.UtcNow,
                IsInvitationAccepted = false
            };

            var role = await _roleManager.FindByNameAsync(Constants.IdentityRole.User);

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new ForbiddenAccessException("");
            }

            await _userManager.AddToRoleAsync(user, role?.Name);

            await UserInvitationEmailSend(origin, user);
        }
        return new ApiResponse<string>
        {
            Success = true,
            Data = "User is invited successfully!",
            StatusCode = HttpStatusCodes.OK,
            Message = "User is invited successfully!"
        };
    }

    private async Task UserInvitationEmailSend(string origin, ApplicationUser user)
    {
        string userInvitedEmailUri = await GetUserInvitedEmailUriAsync(user, origin);

        EmailContent eMailModel = new EmailContent(origin, userInvitedEmailUri)
        {
            Subject = "User Invitation",
            HeyUserName = user.FirstName + " " + user.LastName,
            YourDomain = _configuration.GetSection("CorsSettings")["CorsURLs"],
            ButtonText = "Accept Invitation",
            RowData = new List<string> { "Join our exclusive plateform and unlock premium features.", "Connect with like-minded individuals and expand your network.", "Experience the power of collaboration. Click the button to accept the invitation!" },
            ButtonAnchorUrl = userInvitedEmailUri,
        };

        var mailRequest = new MailRequest(
            new List<string> { user.Email },
            "User Invitation",
            _templateService.GenerateDefaultEmailTemplate(eMailModel));
        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
    }

    public async Task<ApiResponse<string>> AcceptInvitationAsync(AcceptUserInvitationRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);

        _ = user ?? throw new NotFoundException("User not null", user);

        var resultPW = await _userManager.AddPasswordAsync(user, request.Password!);

        if (!resultPW.Succeeded)
        {
            throw new Exception(string.Join(", ", resultPW.Errors.Select(e => e.Description)));
        }

        user.IsInvitationAccepted = user.IsActive = user.EmailConfirmed = true;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new ForbiddenAccessException("Accept Invite failed");
        }

        return new ApiResponse<string>
        {
            Success = true,
            Data = "Invitation Accepted successfully!",
            StatusCode = HttpStatusCodes.OK,
            Message = "Invitation Accepted successfully!"
        };
    }

    public async Task<ApiResponse<UserInviteDto>> GetAcceptInviteDetailsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException("Invitation Not Found.", userId);

        var invitedUser = await _userManager.FindByIdAsync(user.InvitedBy.ToString());

        _ = invitedUser ?? throw new NotFoundException("Invitation Not Found.", userId);

        var useRole = await _db.UserRoles.FirstOrDefaultAsync(u => u.UserId == user.Id);

        _ = useRole ?? throw new NotFoundException("Invitation Not Found.", userId);

        var role = await _roleManager.FindByIdAsync(useRole.RoleId);

        _ = role ?? throw new NotFoundException("Invitation Not Found.", userId);

        var userInviteDto = new UserInviteDto()
        {
            InvitedBy = invitedUser.FirstName + " " + invitedUser.LastName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };

        return new ApiResponse<UserInviteDto>
        {
            Success = true,
            Data = userInviteDto,
            StatusCode = user.IsInvitationAccepted ? HttpStatusCodes.Conflict : HttpStatusCodes.OK,
        };
    }
}
