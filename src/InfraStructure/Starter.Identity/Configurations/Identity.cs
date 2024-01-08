using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Starter.Domain.Common;
using Starter.Identity.Models;

namespace Starter.Identity.Configurations;
internal sealed class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .ToTable("Users", SchemaNames.Identity);

    }
}

internal sealed class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder
            .ToTable("Roles", SchemaNames.Identity);
    }
}

internal sealed class ApplicationRoleClaimConfig : IEntityTypeConfiguration<ApplicationRoleClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
    {
        builder
            .ToTable("RoleClaims", SchemaNames.Identity);
    }
}

internal sealed class IdentityUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder) =>
        builder.ToTable("UserRoles", SchemaNames.Identity);
}

internal sealed class IdentityUserClaimConfig : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder) =>
        builder.ToTable("UserClaims", SchemaNames.Identity);
}

internal sealed class IdentityUserLoginConfig : IEntityTypeConfiguration<ApplicationUserLogin>
{
    public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder) =>
        builder.ToTable("UserLogins", SchemaNames.Identity);
}

internal sealed class IdentityUserTokenConfig : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder) =>
        builder.ToTable("UserTokens", SchemaNames.Identity);
}
