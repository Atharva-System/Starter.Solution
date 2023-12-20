using Starter.Application.Contracts.Mailing.Models;
using Starter.Application.Contracts.Mailing;
using Starter.Application.Features.Common;
using Starter.Application.Features.Users.Invite;
using Starter.Identity.Models;
using Starter.Application.Exceptions;

namespace Starter.Identity.Services;
public partial class UsersService
{
    public async Task<ApiResponse<string>> CreateInvitationAsync(CreateUserInvitation request, string origin)
    {
        var applicationUser = await ExistsUserWithEmailAsync(request.Email);
        if (applicationUser == true)
        {
            throw new ForbiddenAccessException("Invitation already sent.");
        }
        else
        {
            var user = new ApplicationUser()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                IsActive = true,
                InvitedBy = new Guid(_currentUserService.UserId),
                InvitedDate = DateTime.UtcNow,
                IsInvitationAccepted = false
            };

            var role = await _roleManager.FindByIdAsync(request.RoleId);

            _ = role ?? throw new NotFoundException("Role", request.RoleId);

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new ForbiddenAccessException("Validation Errors Occurred.");
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
            YourDomain = "Atharva",
            ButtonText = "Accept Invitation",
            RowData = new List<string> { "Join our exclusive plateform and unlock premium features.", "Connect with like-minded individuals and expand youu network.", "Experience the power of collaboration. Click the button to accept the invitation!" },
            ButtonAnchorUrl = userInvitedEmailUri,
        };

        var mailRequest = new MailRequest(
            new List<string> { user.Email },
            "User Invitation",
            _templateService.GenerateDefaultEmailTemplate(eMailModel));
        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
    }
}
