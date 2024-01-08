using Microsoft.AspNetCore.Identity;

namespace Starter.Identity.Models;
public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }

    public ApplicationRole(string name, string? description = null)
        : base(name)
    {
        Description = description;
        NormalizedName = name.ToUpperInvariant();
        CreatedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
    }

    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    public Guid LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedOn { get; set; } = DateTime.UtcNow;
    public Guid? DeletedBy { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; } = false;
}
