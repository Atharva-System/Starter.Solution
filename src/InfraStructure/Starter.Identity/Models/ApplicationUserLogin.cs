using Microsoft.AspNetCore.Identity;

namespace Starter.Identity.Models;
public class ApplicationUserLogin : IdentityUserLogin<string>
{
    public DateTime LastLoginDateTime { get; set; } = DateTime.UtcNow;

    public DateTime? LogOutDateTime { get; set; } = DateTime.UtcNow;

    public string IpAddress { get; set; } = string.Empty;
}

