using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Backend.Infrastructure.SecurityHeaders;

internal static class Startup
{
    internal static IApplicationBuilder UseSecurityHeaders(
        this IApplicationBuilder app,
        IConfiguration config
    )
    {
        var settings = config
            .GetSection(nameof(SecurityHeaderSettings))
            .Get<SecurityHeaderSettings>();

        if (settings?.Enable is true)
        {
            app.Use(
                async (context, next) =>
                {
                    if (!string.IsNullOrWhiteSpace(settings.XFrameOptions))
                    {
                        context.Response.Headers[HeaderNames.Xframeoptions] =
                            settings.XFrameOptions;
                    }

                    if (!string.IsNullOrWhiteSpace(settings.XContentTypeOptions))
                    {
                        context.Response.Headers[HeaderNames.Xcontenttypeoptions] =
                            settings.XContentTypeOptions;
                    }

                    if (!string.IsNullOrWhiteSpace(settings.ReferrerPolicy))
                    {
                        context.Response.Headers[HeaderNames.Referrerpolicy] =
                            settings.ReferrerPolicy;
                    }

                    if (!string.IsNullOrWhiteSpace(settings.PermissionsPolicy))
                    {
                        context.Response.Headers[HeaderNames.Permissionspolicy] =
                            settings.PermissionsPolicy;
                    }

                    if (!string.IsNullOrWhiteSpace(settings.SameSite))
                    {
                        context.Response.Headers[HeaderNames.Samesite] = settings.SameSite;
                    }

                    if (!string.IsNullOrWhiteSpace(settings.XxssProtection))
                    {
                        context.Response.Headers[HeaderNames.Xxssprotection] =
                            settings.XxssProtection;
                    }

                    await next();
                }
            );
        }

        return app;
    }
}
