﻿using Backend.Infrastructure.Common;
using Backend.Shared.Authorization;
using Backend.Shared.Multitenancy;
using Finbuckle.MultiTenant;
using Hangfire.Client;
using Hangfire.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.BackgroundJobs;

public class DefaultJobFilter(IServiceProvider services) : IClientFilter
{
    private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

    public void OnCreating(CreatingContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        Logger.InfoFormat(
            "Set TenantId and UserId parameters to job {0}.{1}...",
            context.Job.Method.ReflectedType?.FullName,
            context.Job.Method.Name
        );

        using var scope = services.CreateScope();

        var httpContext = scope
            .ServiceProvider.GetRequiredService<IHttpContextAccessor>()
            ?.HttpContext;
        _ =
            httpContext
            ?? throw new InvalidOperationException("Can't create a TenantJob without HttpContext.");

        var tenantInfo = scope.ServiceProvider.GetRequiredService<ITenantInfo>();
        context.SetJobParameter(MultitenancyConstants.TenantIdName, tenantInfo);

        var userId = httpContext.User.GetUserId();
        context.SetJobParameter(QueryStringKeys.UserId, userId);
    }

    public void OnCreated(CreatedContext context) =>
        Logger.InfoFormat(
            "Job created with parameters {0}",
            context
                .Parameters.Select(x => x.Key + "=" + x.Value)
                .Aggregate((s1, s2) => s1 + ";" + s2)
        );
}
