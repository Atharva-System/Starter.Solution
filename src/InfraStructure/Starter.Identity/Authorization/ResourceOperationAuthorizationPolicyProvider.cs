using Microsoft.AspNetCore.Authorization;

namespace Starter.Identity.Authorization;
public class ResourceOperationAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    const string POLICY_PREFIX = "ResourceOperation";

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
    }

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
    {
        return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
    }

    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
        {
            string permissionString = policyName.Substring(POLICY_PREFIX.Length);

            // Check if the permission string is valid (customize as needed)
            if (IsValidPermission(permissionString))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.RequireClaim(CustomClaimTypes.Permission, $"{Resources.TodoItem}.{permissionString}");
                return Task.FromResult(policy.Build());
            }
        }

        return Task.FromResult<AuthorizationPolicy>(null);
    }

    private bool IsValidPermission(string permission)
    {
        // Add your validation logic here
        // Example: Check if the permission string is not empty
        return !string.IsNullOrEmpty(permission);
    }
}
