using Microsoft.AspNetCore.Identity;

namespace Starter.Identity.Models;
public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public string? CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
}
