﻿using Microsoft.AspNetCore.Authorization;

namespace Starter.Identity.Authorizations.Permissions;
public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = Permission.NameFor(action, resource);
}
