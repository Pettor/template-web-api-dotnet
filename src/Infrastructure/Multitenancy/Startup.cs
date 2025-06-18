using Backend.Application.Multitenancy;
using Backend.Application.Multitenancy.Interfaces;
using Backend.Infrastructure.Persistence;
using Backend.Shared.Authorization;
using Backend.Shared.Multitenancy;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Multitenancy;

internal static class Startup
{
    internal static IServiceCollection AddMultitenancy(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        // TODO: We should probably add specific dbprovider/connectionstring setting for the tenantDb with a fallback to the main databasesettings
        var databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        var rootConnectionString = databaseSettings!.ConnectionString;
        if (string.IsNullOrEmpty(rootConnectionString))
            throw new InvalidOperationException("DB ConnectionString is not configured.");
        var dbProvider = databaseSettings.DbProvider;
        if (string.IsNullOrEmpty(dbProvider))
            throw new InvalidOperationException("DB Provider is not configured.");

        return services
            .AddDbContext<TenantDbContext>(m => m.UseDatabase(dbProvider, rootConnectionString))
            .AddMultiTenant<TenantInfo>()
            .WithClaimStrategy(ApiClaims.Tenant)
            .WithHeaderStrategy(MultitenancyConstants.TenantIdName)
            .WithStrategy<QueryStringStrategy>(
                ServiceLifetime.Singleton,
                MultitenancyConstants.TenantIdName
            )
            .WithEFCoreStore<TenantDbContext, TenantInfo>()
            .Services.AddScoped<ITenantService, TenantService>()
            .AddScoped<ITenantInfo, TenantInfo>();
    }

    internal static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app) =>
        app.UseMultiTenant();
}

public class QueryStringStrategy : IMultiTenantStrategy
{
    private readonly string _queryStringKey;

    public QueryStringStrategy(string queryStringKey)
    {
        _queryStringKey = queryStringKey;
    }

    public Task<string?> GetIdentifierAsync(object context)
    {
        if (context is not HttpContext httpContext)
        {
            return Task.FromResult((string?)null);
        }

        httpContext.Request.Query.TryGetValue(_queryStringKey, out var tenantIdParam);
        return Task.FromResult((string?)tenantIdParam.FirstOrDefault());
    }
}
