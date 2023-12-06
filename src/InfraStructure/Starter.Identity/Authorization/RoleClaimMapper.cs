namespace Starter.Identity.Authorization;
//public static class RoleClaimMapper
//{
//    // ... (other methods)

//    public static string MapResourceToClaim(string resourceName, string permission)
//    {
//        // Map resources to claims as needed
//        return $"{resourceName}.{permission}";
//    }
//}


public static class RoleClaimMapper
{
    public static string MapRoleToClaim(string roleName)
    {
        // Map roles to claims as needed
        switch (roleName)
        {
            case Roles.Administrator:
                return Permissions.Create; // Administrators have create permission
            case Roles.User:
                return Permissions.Read; // Users have read permission
            // Add more role-to-claim mappings as needed
            default:
                return string.Empty;
        }
    }

    public static string MapResourceToClaim(string resourceName, string permission)
    {
        // Map resources to claims as needed
        return $"{resourceName}.{permission}";
    }
}
