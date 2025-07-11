﻿using Backend.Infrastructure.Auth;
using Backend.Infrastructure.Common;
using Backend.Infrastructure.Multitenancy;
using Backend.Shared.Multitenancy;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using TenantInfo = Backend.Infrastructure.Multitenancy.TenantInfo;

namespace Backend.Infrastructure.BackgroundJobs;

public class DefaultJobActivator(IServiceScopeFactory scopeFactory) : JobActivator
{
    private readonly IServiceScopeFactory _scopeFactory =
        scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));

    public override JobActivatorScope BeginScope(PerformContext context) =>
        new Scope(context, _scopeFactory.CreateScope());

    private class Scope : JobActivatorScope, IServiceProvider
    {
        private readonly PerformContext _context;
        private readonly IServiceScope _scope;

        public Scope(PerformContext context, IServiceScope scope)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));

            ReceiveParameters();
        }

        private void ReceiveParameters()
        {
            var tenantInfo = _context.GetJobParameter<TenantInfo>(
                MultitenancyConstants.TenantIdName
            );
            if (tenantInfo is not null)
            {
                var multiTenantContextAccessor =
                    _scope.ServiceProvider.GetRequiredService<IMultiTenantContextAccessor>();
                if (
                    multiTenantContextAccessor.MultiTenantContext
                    is MultiTenantContext<TenantInfo> context
                )
                {
                    context.TenantInfo = tenantInfo;
                }
            }

            var userId = _context.GetJobParameter<string>(QueryStringKeys.UserId);
            if (!string.IsNullOrEmpty(userId))
            {
                _scope
                    .ServiceProvider.GetRequiredService<ICurrentUserInitializer>()
                    .SetCurrentUserId(userId);
            }
        }

        public override object Resolve(Type type) =>
            ActivatorUtilities.GetServiceOrCreateInstance(this, type);

        object? IServiceProvider.GetService(Type serviceType) =>
            serviceType == typeof(PerformContext)
                ? _context
                : _scope.ServiceProvider.GetService(serviceType);
    }
}
