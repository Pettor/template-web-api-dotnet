using Ardalis.Specification.EntityFrameworkCore;
using Backend.Application.Common.Caching;
using Backend.Application.Common.Events;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.FileStorage;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Mailing;
using Backend.Application.Common.Models;
using Backend.Application.Common.Specification;
using Backend.Application.Identity.Users;
using Backend.Domain.Identity;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.Persistence.Context;
using Backend.Shared.Authorization;
using Finbuckle.MultiTenant;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Identity;

internal partial class UserService(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext db,
    IStringLocalizer<UserService> localizer,
    IJobService jobService,
    IMailService mailService,
    IEmailTemplateService templateService,
    IFileStorageService fileStorage,
    IEventPublisher events,
    ICacheService cache,
    ICacheKeyService cacheKeys,
    ITenantInfo currentTenant,
    IOptions<SecuritySettings> securitySettings
) : IUserService
{
    private readonly SecuritySettings _securitySettings = securitySettings.Value;

    public async Task<PaginationResponse<UserDetailsDto>> SearchAsync(
        UserListFilter filter,
        CancellationToken cancellationToken
    )
    {
        var spec = new EntitiesByPaginationFilterSpec<ApplicationUser>(filter);

        var users = await userManager
            .Users.WithSpecification(spec)
            .ProjectToType<UserDetailsDto>()
            .ToListAsync(cancellationToken);
        var count = await userManager.Users.CountAsync(cancellationToken);

        return new PaginationResponse<UserDetailsDto>(
            users,
            count,
            filter.PageNumber,
            filter.PageSize
        );
    }

    public async Task<bool> ExistsWithNameAsync(string name)
    {
        EnsureValidTenant();
        return await userManager.FindByNameAsync(name) is not null;
    }

    public async Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
    {
        EnsureValidTenant();
        return await userManager.FindByEmailAsync(email.Normalize()) is ApplicationUser user
            && user.Id != exceptId;
    }

    public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
    {
        EnsureValidTenant();
        return await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber)
                is ApplicationUser user
            && user.Id != exceptId;
    }

    private void EnsureValidTenant()
    {
        if (string.IsNullOrWhiteSpace(currentTenant?.Id))
        {
            throw new UnauthorizedException(localizer["tenant.invalid"]);
        }
    }

    public async Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken) =>
        (await userManager.Users.AsNoTracking().ToListAsync(cancellationToken)).Adapt<
            List<UserDetailsDto>
        >();

    public Task<int> GetCountAsync(CancellationToken cancellationToken) =>
        userManager.Users.AsNoTracking().CountAsync(cancellationToken);

    public async Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await userManager
            .Users.AsNoTracking()
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(localizer["User Not Found."]);

        return user.Adapt<UserDetailsDto>();
    }

    public async Task ToggleStatusAsync(
        ToggleUserStatusRequest request,
        CancellationToken cancellationToken
    )
    {
        var user = await userManager
            .Users.Where(u => u.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(localizer["User Not Found."]);

        var isAdmin = await userManager.IsInRoleAsync(user, ApiRoles.Admin);
        if (isAdmin)
        {
            throw new ConflictException(
                localizer["Administrators Profile's Status cannot be toggled"]
            );
        }

        user.IsActive = request.ActivateUser;

        await userManager.UpdateAsync(user);

        await events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
    }
}
