using Microsoft.AspNetCore.Identity;
using Starter.Domain.Common.Contracts;

namespace Starter.Identity.Models;

public class ApplicationUser : IdentityUser, IAuditableEntity, ISoftDelete
{

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTimeOffset ModifiedOn { get; set; }
    public DateTimeOffset DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public Guid InvitedBy { get; set; }
    public DateTime InvitedDate { get; set; }
    public bool IsInvitationAccepted { get; set; } = false;
    public string? Culture { get; set; }
    public string? VerificationCode { get; set; }
    public bool? IsSuperAdmin { get; set; } = false;


}
