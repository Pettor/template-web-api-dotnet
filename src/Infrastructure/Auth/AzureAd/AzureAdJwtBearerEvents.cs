using System.Security.Claims;
using Backend.Application.Common.Exceptions;
using Backend.Application.Identity.Users;
using Backend.Infrastructure.Multitenancy;
using Backend.Shared.Authorization;
using Backend.Shared.Multitenancy;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Serilog;
using TenantInfo = Backend.Infrastructure.Multitenancy.TenantInfo;

namespace Backend.Infrastructure.Auth.AzureAd;

internal class AzureAdJwtBearerEvents(ILogger logger, IConfiguration config) : JwtBearerEvents
{
    public override Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        logger.AuthenticationFailed(context.Exception);
        return base.AuthenticationFailed(context);
    }

    public override Task MessageReceived(MessageReceivedContext context)
    {
        logger.TokenReceived();
        return base.MessageReceived(context);
    }

    /// <summary>
    /// This method contains the logic that validates the user's tenant and normalizes claims.
    /// </summary>
    /// <param name="context">The validated token context.</param>
    /// <returns>A task.</returns>
    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var principal = context.Principal;
        var issuer = principal?.GetIssuer();
        var objectId = principal?.GetObjectId();
        logger.TokenValidationStarted(objectId, issuer);

        if (principal is null || issuer is null || objectId is null)
        {
            logger.TokenValidationFailed(objectId, issuer);
            throw new UnauthorizedException("Authentication Failed.");
        }

        // Lookup the tenant using the issuer.
        // TODO: we should probably cache this (root tenant and tenant per issuer)
        var tenantDb = context.HttpContext.RequestServices.GetRequiredService<TenantDbContext>();
        var tenant =
            issuer == config["SecuritySettings:AzureAd:RootIssuer"]
                ? await tenantDb.TenantInfo.FindAsync(MultitenancyConstants.Root.Id)
                : await tenantDb.TenantInfo.FirstOrDefaultAsync(t => t.Issuer == issuer);

        if (tenant is null)
        {
            logger.TokenValidationFailed(objectId, issuer);

            // The caller was not from a trusted issuer - throw to block the authentication flow.
            throw new UnauthorizedException("Authentication Failed.");
        }

        // The caller comes from an admin-consented, recorded issuer.
        var identity = principal.Identities.First();

        // Adding tenant claim.
        identity.AddClaim(new Claim(ApiClaims.Tenant, tenant.Id));

        // Set new tenant info to the HttpContext so the right connectionstring is used.
        context.HttpContext.TrySetTenantInfo(tenant, false);

        // Lookup local user or create one if none exist.
        var userId = await context
            .HttpContext.RequestServices.GetRequiredService<IUserService>()
            .GetOrCreateFromPrincipalAsync(principal);

        // We use the nameidentifier claim to store the user id.
        var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        identity.TryRemoveClaim(idClaim);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));

        // And the email claim for the email.
        var upnClaim = principal.FindFirst(ClaimTypes.Upn);
        if (upnClaim is not null)
        {
            var emailClaim = principal.FindFirst(ClaimTypes.Email);
            identity.TryRemoveClaim(emailClaim);
            identity.AddClaim(new Claim(ClaimTypes.Email, upnClaim.Value));
        }

        logger.TokenValidationSucceeded(objectId, issuer);
    }
}

public static class HttpContextExtensions
{
    public static void TrySetTenantInfo(
        this HttpContext httpContext,
        TenantInfo tenantInfo,
        bool throwIfNull = true
    )
    {
        var multiTenantContextAccessor =
            httpContext.RequestServices.GetRequiredService<IMultiTenantContextAccessor>();
        if (multiTenantContextAccessor.MultiTenantContext is MultiTenantContext<TenantInfo> context)
        {
            context.TenantInfo = tenantInfo;
        }
        else if (throwIfNull)
        {
            throw new InvalidOperationException("MultiTenantContext is not available.");
        }
    }
}
