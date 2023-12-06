using Microsoft.AspNetCore.Authorization;

namespace Starter.Identity.Authorization;
public class ResourceOperationRequirement : IAuthorizationRequirement
{
    public string Resource { get; }
    public string Operation { get; }

    public ResourceOperationRequirement(string resource, string operation)
    {
        Resource = resource;
        Operation = operation;
    }
}
