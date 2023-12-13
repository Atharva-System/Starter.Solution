using Microsoft.AspNetCore.Identity;

namespace Starter.Identity.Models;

public class ApplicationUser : IdentityUser
{

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; private set; }
    public Guid LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public Guid InvitedBy { get; set; }
    public DateTime InvitedDate { get; set; }
    public bool IsInvitationAccepted { get; set; } = false;
    public string? Culture { get; set; }
    public string? VerificationCode { get; set; }
    public bool? IsSuperAdmin { get; set; } = false;


}
