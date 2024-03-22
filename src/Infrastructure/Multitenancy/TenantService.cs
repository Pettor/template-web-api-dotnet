using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Persistence;
using Backend.Application.Multitenancy;
using Backend.Application.Multitenancy.Entities;
using Backend.Application.Multitenancy.Interfaces;
using Backend.Application.Multitenancy.Queries.Create;
using Backend.Infrastructure.Persistence;
using Backend.Infrastructure.Persistence.Initialization;
using Finbuckle.MultiTenant;
using Mapster;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Multitenancy;

internal class TenantService(
    IMultiTenantStore<TenantInfo> tenantStore,
    IConnectionStringSecurer csSecurer,
    IDatabaseInitializer dbInitializer,
    IStringLocalizer<TenantService> localizer,
    IOptions<DatabaseSettings> dbSettings)
    : ITenantService
{
    private readonly DatabaseSettings _dbSettings = dbSettings.Value;

    public async Task<List<TenantDto>> GetAllAsync()
    {
        var tenants = (await tenantStore.GetAllAsync()).Adapt<List<TenantDto>>();
        tenants.ForEach(t => t.ConnectionString = csSecurer.MakeSecure(t.ConnectionString));
        return tenants;
    }

    public async Task<bool> ExistsWithIdAsync(string id) =>
        await tenantStore.TryGetAsync(id) is not null;

    public async Task<bool> ExistsWithNameAsync(string name) =>
        (await tenantStore.GetAllAsync()).Any(t => t.Name == name);

    public async Task<TenantDto> GetByIdAsync(string id) =>
        (await GetTenantInfoAsync(id))
            .Adapt<TenantDto>();

    public async Task<string> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken)
    {
        if (request.ConnectionString?.Trim() == _dbSettings.ConnectionString?.Trim())
            request.ConnectionString = string.Empty;

        var tenant = new TenantInfo(request.Id, request.Name, request.ConnectionString, request.AdminEmail, request.Issuer);
        await tenantStore.TryAddAsync(tenant);

        // TODO: run this in a hangfire job? will then have to send mail when it's ready or not
        try
        {
            await dbInitializer.InitializeApplicationDbForTenantAsync(tenant, cancellationToken);
        }
        catch
        {
            await tenantStore.TryRemoveAsync(request.Id);
            throw;
        }

        return tenant.Id;
    }

    public async Task<string> ActivateAsync(string id)
    {
        var tenant = await GetTenantInfoAsync(id);

        if (tenant.IsActive)
        {
            throw new ConflictException("Tenant is already Activated.");
        }

        tenant.Activate();

        await tenantStore.TryUpdateAsync(tenant);

        return $"Tenant {id} is now Activated.";
    }

    public async Task<string> DeactivateAsync(string id)
    {
        var tenant = await GetTenantInfoAsync(id);

        if (!tenant.IsActive)
        {
            throw new ConflictException("Tenant is already Deactivated.");
        }

        tenant.Deactivate();

        await tenantStore.TryUpdateAsync(tenant);

        return $"Tenant {id} is now Deactivated.";
    }

    public async Task<string> UpdateSubscription(string id, DateTime extendedExpiryDate)
    {
        var tenant = await GetTenantInfoAsync(id);

        tenant.SetValidity(extendedExpiryDate);

        await tenantStore.TryUpdateAsync(tenant);

        return $"Tenant {id}'s Subscription Upgraded. Now Valid till {tenant.ValidUpto}.";
    }

    private async Task<TenantInfo> GetTenantInfoAsync(string id) =>
        await tenantStore.TryGetAsync(id)
            ?? throw new NotFoundException(string.Format(localizer["entity.notfound"], typeof(TenantInfo).Name, id));
}
