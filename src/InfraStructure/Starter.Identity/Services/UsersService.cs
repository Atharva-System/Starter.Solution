using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Identity;
using Starter.Application.Contracts.Mailing;
﻿using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Application;
using Starter.Application.Contracts.Identity;
using Starter.Application.Contracts.Responses;
using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.Interfaces;
using Starter.Application.Models.Users;
using Starter.Identity.Database;
using Starter.Identity.Models;
using Starter.Application.Contracts.Mailing.Models;
using Starter.Application.Features.Users.Invite;
using System.Drawing;
using Starter.Application.Contracts.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Starter.Identity.Services;
public partial class UsersService(AppIdentityDbContext db, UserManager<ApplicationUser> userManager, IEmailTemplateService templateService, IMailService mailService, IJobService jobService) : IUsersService
{
    private readonly AppIdentityDbContext _db = db;
    //private readonly ICurrentUser _currentUser;
    private readonly IJobService _jobService = jobService;
    private readonly IMailService _mailService = mailService;
    private readonly IEmailTemplateService _templateService = templateService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<ApiResponse<string>> CreateInvitationAsync(CreateUserInvitation request, string origin)
    {
        try
        {
            var user = new ApplicationUser()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                IsActive = true,
                //InvitedBy = _currentUser.GetUserId(),
                InvitedDate = DateTime.UtcNow,
                IsInvitationAccepted = false
            };

            await UserInvitationEmailSend(origin, user);

            //SendAsync

            var response = new ApiResponse<string>()
            {
                Success = true,
                StatusCode = 200,
                Message = "Invitation created successfully",
                Data = "Invitation created successfully",
            };
            return response;
        }
        catch (Exception)
        {

            throw;
        }
        
    }

    private async Task UserInvitationEmailSend(string origin, ApplicationUser user)
    {
        //string buttonAnchorUrl = "User/invite";
        string userInvitedEmailUri = await GetUserInvitedEmailUriAsync(user, origin);

        EmailContent eMailModel = new EmailContent(origin, userInvitedEmailUri)
        {
            Subject = "User Invitation",
            HeyUserName = user.FirstName,
            YourDomain = "Atharva",
            ButtonText = "Accept User",
            RowData = new List<string> { "Data1", "Data2", "Data3" },
            ButtonAnchorUrl = userInvitedEmailUri,
        };

        var mailRequest = new MailRequest(
            new List<string> { user.Email },
            "User Invitation",
            _templateService.GenerateDefaultEmailTemplate(eMailModel));
        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
    }

    private async Task<string> GetUserInvitedEmailUriAsync(ApplicationUser user, string origin)
    {
        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string route = "User/accept-invitation";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), user.Id, user.Id);
        return verificationUri;
    }
public partial class UsersService(AppIdentityDbContext db, ICurrentUserService currentUserService) : IUsersService
{
    private readonly AppIdentityDbContext _db = db;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<ApiResponse<UserDetailsDto>> GetUserDetailsAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await (from u in _db.Users.AsNoTracking()
                          join ur in _db.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                          join r in _db.Roles.AsNoTracking() on ur.RoleId equals r.Id
                          where u.Id == userId
                          select new UserDetailsDto()
                          {
                              Id = u.Id,
                              FirstName = u.FirstName ?? string.Empty,
                              LastName = u.LastName ?? string.Empty,
                              Email = u.Email ?? string.Empty,
                              RoleId = r.Id,
                              RoleName = r.Name ?? string.Empty,
                          }).FirstOrDefaultAsync();

        _ = user ?? throw new NotFoundException("User ", userId);

        var response = new ApiResponse<UserDetailsDto>
        {
            Success = user != null,
            StatusCode = user != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Data = user,
            Message = user != null ? $"User {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} user."
        };
        return response;
    }

    public async Task<IPagedDataResponse<UserListDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken)
    {
        var usersList = await (from u in _db.Users.AsNoTracking()
                               join ur in _db.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                               join r in _db.Roles.AsNoTracking() on ur.RoleId equals r.Id
                               where u.Id != _currentUserService.UserId
                               select new UserListDto()
                               {
                                   Id = u.Id,
                                   FirstName = u.FirstName ?? string.Empty,
                                   LastName = u.LastName ?? string.Empty,
                                   Email = u.Email ?? string.Empty,
                                   FullName = u.FirstName + " " + u.LastName,
                                   Status = u.IsInvitationAccepted == false ? "Invited" : (u.IsActive ? "Active" : "InActive"),
                                   Role = r.Name ?? string.Empty,
                                   CreatedOn = u.CreatedOn
                               }
                    ).ToListAsync<UserListDto>();

        var spec = new GetSearchUserRequestSpec(filter);

        var users = usersList.ApplySpecification(spec);

        int count = usersList.ApplySpecificationCount(spec);

        return new PagedApiResponse<UserListDto>(count, filter.PageNumber, filter.PageSize) { Data = users };
    }
}
