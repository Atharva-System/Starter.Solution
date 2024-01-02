using Starter.Application.Contracts.Mailing.Models;
using Starter.Application.Contracts.Mailing;
using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.Features.Users.Invite;
using Starter.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Starter.Application.Features.Users.AcceptInvite;

namespace Starter.Identity.Services;
public partial class UsersService
{
    public async Task<ApiResponse<string>> CreateInvitationAsync(CreateUserInvitation request, string origin)
    {
        var applicationUser = await ExistsUserWithEmailAsync(request.Email);
        if (applicationUser == true)
        {
            throw new ForbiddenAccessException("");
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

            var role = await _roleManager.FindByIdAsync(request.RoleId);

            _ = role ?? throw new NotFoundException("Role", request.RoleId);

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
            throw new ForbiddenAccessException("");
        }

        user.IsInvitationAccepted = user.IsActive = user.EmailConfirmed = true;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new ForbiddenAccessException("");
        }

        return new ApiResponse<string>
        {
            Success = true,
            Data = "Invitation Accepted successfully!",
            StatusCode = HttpStatusCodes.OK,
            Message = "Invitation Accepted successfully!"
        };
    }
}
