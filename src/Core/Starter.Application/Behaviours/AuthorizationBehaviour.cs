using System.Reflection;
using Starter.Application.Contracts.Application;
using Starter.Application.Contracts.Identity;
using Starter.Application.Exceptions;
using Starter.Application.Security;

namespace Starter.Application.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse>(
        ICurrentUserService user,
        IAuthService identityService) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ICurrentUserService _user = user;
    private readonly IAuthService _authService = identityService;


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
        {
            // Must be authenticated user
            if (_user.UserId == null)
            {
                throw new UnauthorizedAccessException();
            }


            //var currentRole = "Administrator";

            List<string[]> roleClaims = new List<string[]>
            {
                new string[] { "TodoItem", "Create" },

            };

            //"CreateTodoItemCommand"

            //var askingRequest = typeof(TRequest).Name;
            //var authorized = false;

            //foreach (var claim in roleClaims)
            //{
            //    if ($"{claim[1]}_{claim[0]}_Command" == askingRequest)
            //    {
            //        authorized = true;
            //        break;
            //    }
            //}
            //if (!authorized)
            //{
            //    throw new ForbiddenAccessException();
            //}

            // Role-based authorization
            var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.ModuleType) && !string.IsNullOrWhiteSpace(a.Actions));

            if (authorizeAttributesWithRoles.Any())
            {
                var authorized = false;
                foreach (var claim in roleClaims)
                {
                    if (claim[0] == authorizeAttributes.First().ModuleType && claim[1] == authorizeAttributes.First().Actions)
                    {
                        authorized = true;
                        break;
                    }
                }

                //foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                //{
                //    foreach (var role in roles)
                //    {
                //        var isInRole = await _authService.IsInRoleAsync(_user.UserId, role.Trim());
                //        if (isInRole)
                //        {
                //            authorized = true;
                //            break;
                //        }
                //    }
                //}




                // Must be a member of at least one role in roles
                if (!authorized)
                {
                    throw new ForbiddenAccessException("");
                }
            }

            // Policy-based authorization
            //var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
            //if (authorizeAttributesWithPolicies.Any())
            //{
            //    foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
            //    {
            //        var authorized = await _authService.AuthorizeAsync(_user.UserId, policy);

            //        if (!authorized)
            //        {
            //            throw new ForbiddenAccessException();
            //        }
            //    }
            //}
        }

        // User is authorized / authorization not required
        return await next();
    }
}
