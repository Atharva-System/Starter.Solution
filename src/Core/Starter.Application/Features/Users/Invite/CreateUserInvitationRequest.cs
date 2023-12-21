using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;

namespace Starter.Application.Features.Users.Invite;

public class CreateUserInvitation
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string RoleId { get; set; } = default!;
}

public class CreateUserInvitationRequest : IRequest<ApiResponse<string>>
{
    public CreateUserInvitation request { get; set; }
    public string Origion { get; set; } = default!;
}
