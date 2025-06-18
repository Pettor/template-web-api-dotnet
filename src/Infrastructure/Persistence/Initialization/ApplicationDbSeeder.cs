using Backend.Infrastructure.Identity;
using Backend.Infrastructure.Multitenancy;
using Backend.Infrastructure.Persistence.Context;
using Backend.Shared.Authorization;
using Backend.Shared.Multitenancy;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Persistence.Initialization;

internal class ApplicationDbSeeder(
    IMultiTenantContextAccessor<TenantInfo> contextAccessor,
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    CustomSeederRunner seederRunner,
    ILogger<ApplicationDbSeeder> logger
)
{
    private readonly TenantInfo _currentTenant = contextAccessor.MultiTenantContext.TenantInfo!;

    public async Task SeedDatabaseAsync(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        await SeedRolesAsync(dbContext);
        await SeedAdminUserAsync();
        await seederRunner.RunSeedersAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(ApplicationDbContext dbContext)
    {
        foreach (var roleName in ApiRoles.DefaultRoles)
        {
            if (
                await roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
                is not ApplicationRole role
            )
            {
                // Create the role
                logger.LogInformation(
                    "Seeding {role} Role for '{tenantId}' Tenant.",
                    roleName,
                    _currentTenant.Id
                );
                role = new ApplicationRole(
                    roleName,
                    $"{roleName} Role for {_currentTenant.Id} Tenant"
                );
                await roleManager.CreateAsync(role);
            }

            switch (roleName)
            {
                // Assign permissions
                case ApiRoles.Basic:
                    await AssignPermissionsToRoleAsync(dbContext, ApiPermissions.Basic, role);
                    break;
                case ApiRoles.Admin:
                {
                    await AssignPermissionsToRoleAsync(dbContext, ApiPermissions.Admin, role);

                    if (_currentTenant.Id == MultitenancyConstants.Root.Id)
                    {
                        await AssignPermissionsToRoleAsync(dbContext, ApiPermissions.Root, role);
                    }

                    break;
                }
            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(
        ApplicationDbContext dbContext,
        IReadOnlyList<ApiPermission> permissions,
        ApplicationRole role
    )
    {
        var currentClaims = await roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (
                !currentClaims.Any(c =>
                    c.Type == ApiClaims.Permission && c.Value == permission.Name
                )
            )
            {
                logger.LogInformation(
                    "Seeding {role} Permission '{permission}' for '{tenantId}' Tenant.",
                    role.Name,
                    permission.Name,
                    _currentTenant.Id
                );
                dbContext.RoleClaims.Add(
                    new ApplicationRoleClaim
                    {
                        RoleId = role.Id,
                        ClaimType = ApiClaims.Permission,
                        ClaimValue = permission.Name,
                        CreatedBy = "ApplicationDbSeeder",
                    }
                );
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private async Task SeedAdminUserAsync()
    {
        if (
            string.IsNullOrWhiteSpace(_currentTenant.Id)
            || string.IsNullOrWhiteSpace(_currentTenant.AdminEmail)
        )
        {
            return;
        }

        if (
            await userManager.Users.FirstOrDefaultAsync(u => u.Email == _currentTenant.AdminEmail)
            is not ApplicationUser adminUser
        )
        {
            var adminUserName = $"{_currentTenant.Id.Trim()}.{ApiRoles.Admin}".ToLowerInvariant();
            adminUser = new ApplicationUser
            {
                FirstName = _currentTenant.Id.Trim().ToLowerInvariant(),
                LastName = ApiRoles.Admin,
                Email = _currentTenant.AdminEmail,
                UserName = adminUserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = _currentTenant.AdminEmail?.ToUpperInvariant(),
                NormalizedUserName = adminUserName.ToUpperInvariant(),
                IsActive = true,
            };

            logger.LogInformation(
                "Seeding Default Admin User for '{tenantId}' Tenant.",
                _currentTenant.Id
            );
            var password = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = password.HashPassword(
                adminUser,
                MultitenancyConstants.DefaultPassword
            );
            await userManager.CreateAsync(adminUser);
        }

        // Assign role to user
        if (!await userManager.IsInRoleAsync(adminUser, ApiRoles.Admin))
        {
            logger.LogInformation(
                "Assigning Admin Role to Admin User for '{tenantId}' Tenant.",
                _currentTenant.Id
            );
            await userManager.AddToRoleAsync(adminUser, ApiRoles.Admin);
        }
    }
}
