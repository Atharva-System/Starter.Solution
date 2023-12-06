using Microsoft.AspNetCore.Authorization;

namespace Starter.Identity.Authorization;
public class ResourceOperationAuthorizationHandler : AuthorizationHandler<ResourceOperationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement)
    {
        // Check if the user has the required claim
        if (context.User.HasClaim(c => c.Type == CustomClaimTypes.Permission && c.Value == $"{requirement.Resource}.{requirement.Operation}"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

